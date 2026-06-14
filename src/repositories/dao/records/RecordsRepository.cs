using Npgsql;

public static class RecordsRepository
{
    /*
    public static async Task<DAOList<CategoryModelList>> List(QueryPage page, Dictionary<string,object?> filters)
    {
        List<string> whereParts = [];
        List<NpgsqlParameter?> parameters = [];

        if (filters.TryGetValue("name", out var name))
        {
            whereParts.Add("c.name ILIKE @nameFilter");
            parameters.Add(new NpgsqlParameter("nameFilter",$"{name}%"));
        }

        if (filters.TryGetValue("parentId", out var parentId))
        {
            whereParts.Add("c.parent_id = @parentIdFilter");
            parameters.Add(new NpgsqlParameter("parentIdFilter",parentId));
        }

        if (filters.TryGetValue("subcategory", out var isSubcategory))
        {
            string subcategoryFilter = (bool)isSubcategory! ? "IS NOT NULL" : "IS NULL";
            whereParts.Add($"c.parent_id {subcategoryFilter}");
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
                c.id, 
                c.name, 
                c.description, 
                c.parent_id,
                cp.name,
                c.created_at
            FROM categories c
            LEFT JOIN categories cp ON c.parent_id = cp.id
            {where}
            {order}
            LIMIT @limit OFFSET @offset";

        string sqlCount = $@"
            SELECT COUNT(*)
            FROM categories c
            {where}";

        await using var conn = await RepositoryHandler.OpenConnection();

        long totalElements;
        await using (var countCmd = new NpgsqlCommand(sqlCount, conn))
        {
            foreach (var parameter in parameters)
                if (parameter is not null)
                    countCmd.Parameters.Add(parameter.Clone());

            totalElements = Convert.ToInt64(await countCmd.ExecuteScalarAsync());
        }

        var listElements = new List<CategoryModelList>();
        await using (var dataCmd = new NpgsqlCommand(sqlData, conn))
        {
            foreach (var parameter in parameters)
                if (parameter is not null)
                    dataCmd.Parameters.Add(parameter.Clone());

            dataCmd.Parameters.AddWithValue("limit", page.Limit);
            dataCmd.Parameters.AddWithValue("offset", page.Limit * (page.Page - 1));

            await using var reader = await dataCmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                listElements.Add(CategoriesRepositoryReader.SerializeList(reader));
        }

        return new DAOList<CategoryModelList>(totalElements, listElements);
    }

    public static async Task<CategoryModel?> Get(long id)
    {
        const string sql = @"
            SELECT 
                c.id, 
                c.name,
                c.description,
                c.parent_id,
                cp.name as parent_name,
                c.created_at
            FROM categories as c
            LEFT JOIN categories cp ON c.parent_id = cp.id
            WHERE c.id = @id";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        var category = CategoriesRepositoryReader.Serialize(reader);
        await reader.CloseAsync();
        return category;
    }

    public static async Task<bool> Delete(long id)
    {
        const string sql = "DELETE FROM categories WHERE id = @id";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }

    public static async Task<CategoryModelWithSubcategoryCount?> GetWithSubcategoryCountLockWriter(long id, NpgsqlTransaction tx)
    {
        const string sql = @"
            SELECT 
                c.id, 
                c.name, 
                c.description, 
                c.parent_id,
                cp.name,
                c.created_at,
                (
                    SELECT COUNT(*)
                    FROM categories c2
                    WHERE c2.parent_id = c.id
                ) AS children_count
            FROM categories as c
            LEFT JOIN categories cp ON c.parent_id = cp.id
            WHERE c.id = @id
            FOR UPDATE OF c";

        await using var cmd = new NpgsqlCommand(sql, tx.Connection, tx);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        var category = CategoriesRepositoryReader.SerializeWithSubcategoryCount(reader);
        await reader.CloseAsync();
        return category;
    }

    public static async Task<CategoryModelParent?> GetParentLockReader(long id, NpgsqlTransaction tx)
    {
        const string sql = @"
            SELECT
                id,
                name,
                parent_id IS NOT NULL AS has_parent
            FROM categories
            WHERE id = @id
            FOR SHARE";

        await using var cmd = new NpgsqlCommand(sql, tx.Connection, tx);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        var category = CategoriesRepositoryReader.SerializeParent(reader);
        await reader.CloseAsync();
        return category;
    }

    public static async Task<long?> InsertWithTransaction(Record record, NpgsqlTransaction tx) 
    {
        const string sql = @"
            INSERT INTO records
                (description, date, money_total, invisible, public, createdAt, updatedAt, deletedAt, status)
            VALUES
                (@description, @date, @moneyTotal, @invisible, @public, @createdAt, @updatedAt, @deletedAt, @status)
            RETURNING id";

        await using var cmd = new NpgsqlCommand(sql, tx.Connection, tx);

        cmd.Parameters.AddWithValue("description", record.Description is null ? DBNull.Value : record.Description);
        cmd.Parameters.AddWithValue("date", record.Date);
        cmd.Parameters.AddWithValue("moneyTotal", record.TotalMoney);
        cmd.Parameters.AddWithValue("invisible", record.IsInvisible);
        cmd.Parameters.AddWithValue("public", record.IsPublic);
        cmd.Parameters.AddWithValue("createdAt", record.CreatedAt);
        cmd.Parameters.AddWithValue("updatedAt", record.UpdatedAt);
        cmd.Parameters.AddWithValue("deletedAt", record.DeletedAt is null ? DBNull.Value : record.DeletedAt);
        cmd.Parameters.AddWithValue("status", RecordsRepositoryStatus.ConvertStatus(record));

        var result = await cmd.ExecuteScalarAsync();
        if (result is null || result == DBNull.Value)
            return null;

        return Convert.ToInt64(result);
    }*/

    /*
    public static async Task<bool> UpdateWithTransaction(Category category, NpgsqlTransaction tx)
    {
        const string sql = @"
            UPDATE categories
            SET
                name = @name,
                description = @description,
                parent_id = @parentId
            WHERE id = @id";

        await using var cmd = new NpgsqlCommand(sql, tx.Connection, tx);

        cmd.Parameters.AddWithValue("id", category.ID);
        cmd.Parameters.AddWithValue("name", category.Name);
        cmd.Parameters.AddWithValue("description", category.Description is null ? DBNull.Value : category.Description);
        cmd.Parameters.AddWithValue("parentId", category.ParentID is null ? DBNull.Value : category.ParentID);

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }*/
}