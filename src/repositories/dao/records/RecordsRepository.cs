using System.Text;
using Npgsql;

public static class RecordRepository
{
    public static async Task<DAOList<Record>> List(QueryPage page, Dictionary<string,object?> filters)
    {
        List<string> whereParts = [];
        List<NpgsqlParameter?> parameters = [];

        if (filters.TryGetValue("status", out var status))
        {
            List<string> sb = [];
            foreach(string s in (List<object>)status!)
            {
                var extractedStatus = RecordStatusFormatter.Parse(s);
                if (extractedStatus is not null)
                {
                    sb.Add($"status = {RecordStatusFormatter.Import((RecordStatus)extractedStatus)}");
                }
            }

            if (sb.Count > 0)
            {
                whereParts.Add(string.Join(" OR ", sb));
                parameters.Add(null);
            }
        }

        if (filters.TryGetValue("workspace", out var workspace))
        {
            whereParts.Add("workspace = @workspace");
            parameters.Add(new NpgsqlParameter("workspace",$"{workspace}"));
        }

        if (filters.TryGetValue("minDate", out var minDate))
        {
            whereParts.Add("date >= @minDate");
            parameters.Add(new NpgsqlParameter("minDate",(DateOnly)minDate!));
        }

        if (filters.TryGetValue("maxDate", out var maxDate))
        {
            whereParts.Add("date <= @maxDate");
            parameters.Add(new NpgsqlParameter("maxDate",(DateOnly)maxDate!));
        }

        if (filters.TryGetValue("public", out var isPublic))
        {
            string isPublicFilter = (bool)isPublic! ? "TRUE" : "FALSE";
            whereParts.Add($"public = {isPublicFilter}");
            parameters.Add(null);
        }

        if (filters.TryGetValue("invisible", out var invisible))
        {
            string invisibleFilter = (bool)invisible! ? "TRUE" : "FALSE";
            whereParts.Add($"invisible = {invisibleFilter}");
            parameters.Add(null);
        }

        if (filters.TryGetValue("onlyRevenues", out var onlyRevenues) && (bool)onlyRevenues! == true)
        {
            whereParts.Add($"value > 0");
            parameters.Add(null);
        }

        if (filters.TryGetValue("onlyExpenses", out var onlyExpenses) && (bool)onlyExpenses! == true)
        {
            whereParts.Add($"value < 0");
            parameters.Add(null);
        }

        string where = whereParts.Count > 0
            ? $"WHERE {string.Join(" AND ", whereParts)}"
            : "";

        string order = "";
        if (page.Sort.Count > 0)
        {
            List<string> orderParts = [];
            foreach (var item in page.Sort)
            {
                string direction = item.IsAsc ? "ASC" : "DESC";
                orderParts.Add($"{item.Value} {direction}");
            }
            order = $"ORDER BY {string.Join(", ", orderParts)}";
        }

        string sqlData = $@"
            SELECT 
                id, 
                note,
                value,
                date,
                workspace,
                invisible,
                public,
                creation_date,
                updated_date,
                deletion_date,
                status
            FROM records
            {where}
            {order}
            LIMIT @limit OFFSET @offset";

        string sqlCount = $@"
            SELECT COUNT(*)
            FROM records
            {where}";

        Console.WriteLine(sqlData);

        await using var conn = await RepositoryHandler.OpenConnection();

        long totalElements;
        await using (var countCmd = new NpgsqlCommand(sqlCount, conn))
        {
            foreach (var parameter in parameters)
                if (parameter is not null)
                    countCmd.Parameters.Add(parameter.Clone());

            totalElements = Convert.ToInt64(await countCmd.ExecuteScalarAsync());
        }

        var listElements = new List<Record>();
        await using (var dataCmd = new NpgsqlCommand(sqlData, conn))
        {
            foreach (var parameter in parameters)
                if (parameter is not null)
                    dataCmd.Parameters.Add(parameter.Clone());

            dataCmd.Parameters.AddWithValue("limit", page.Limit);
            dataCmd.Parameters.AddWithValue("offset", page.Limit * (page.Page - 1));

            await using var reader = await dataCmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                listElements.Add(RecordsRepositoryReader.Serialize(reader));
        }

        return new DAOList<Record>(totalElements, listElements);
    }

    public static async Task<Record?> Get(long id)
    {
        const string sql = @"
            SELECT 
                id, 
                note,
                value,
                date,
                workspace,
                invisible,
                public,
                creation_date,
                updated_date,
                deletion_date,
                status
            FROM records
            WHERE id = @id";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        var record = RecordsRepositoryReader.Serialize(reader);
        await reader.CloseAsync();
        return record;
    }

    public static async Task<bool> Delete(long id)
    {
        const string sql = "DELETE FROM records WHERE id = @id";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }

    public static async Task<long?> InsertWithTransaction(Record record, NpgsqlTransaction tx) 
    {
        const string sql = @"
            INSERT INTO records
                (note, value, date, workspace, invisible, public, creation_date, updated_date, deletion_date, status)
            VALUES
                (@note, @value, @date, @workspace, @invisible, @public, @creationDate, @updatedDate, @deletionDate, @status)
            RETURNING id";

        await using var cmd = new NpgsqlCommand(sql, tx.Connection, tx);

        cmd.Parameters.AddWithValue("note", record.Note is null ? DBNull.Value : record.Note);
        cmd.Parameters.AddWithValue("value", record.Value);
        cmd.Parameters.AddWithValue("date", record.Date);
        cmd.Parameters.AddWithValue("workspace", record.Workspace);
        cmd.Parameters.AddWithValue("invisible", record.Invisible);
        cmd.Parameters.AddWithValue("public", record.Public);
        cmd.Parameters.AddWithValue("creationDate", record.CreationDate);
        cmd.Parameters.AddWithValue("updatedDate", record.UpdatedDate);
        cmd.Parameters.AddWithValue("deletionDate", record.DeletedDate is null ? DBNull.Value : record.DeletedDate);
        cmd.Parameters.AddWithValue("status", RecordStatusFormatter.Import(record.Status));

        var result = await cmd.ExecuteScalarAsync();
        if (result is null || result == DBNull.Value)
            return null;

        return Convert.ToInt64(result);
    }

    public static async Task<bool> UpdateWithTransaction(Record record, NpgsqlTransaction tx)
    {
        const string sql = @"
            UPDATE records
            SET
                note = @note,
                value = @value,
                date = @date,
                workspace = @workspace,
                invisible = @invisible,
                public = @public,
                updated_date = @updatedDate,
                deletion_date = @deletionDate,
                status = @status
            WHERE id = @id";

        await using var cmd = new NpgsqlCommand(sql, tx.Connection, tx);

        cmd.Parameters.AddWithValue("id", record.ID);
        cmd.Parameters.AddWithValue("note", record.Note is null ? DBNull.Value : record.Note);
        cmd.Parameters.AddWithValue("value", record.Value);
        cmd.Parameters.AddWithValue("date", record.Date);
        cmd.Parameters.AddWithValue("workspace", record.Workspace);
        cmd.Parameters.AddWithValue("invisible", record.Invisible);
        cmd.Parameters.AddWithValue("public", record.Public);
        cmd.Parameters.AddWithValue("updatedDate", record.UpdatedDate);
        cmd.Parameters.AddWithValue("deletionDate", record.DeletedDate is null ? DBNull.Value : record.DeletedDate);
        cmd.Parameters.AddWithValue("status", RecordStatusFormatter.Import(record.Status));

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }
}