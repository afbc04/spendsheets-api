using Npgsql;

public static class TagsRepository
{
    /*
    public static async Task<DAOList<TagModelList>> List(QueryPage page, Dictionary<string,object?> filters)
    {
        List<string> whereParts = [];
        List<NpgsqlParameter> parameters = [];

        if (filters.TryGetValue("name", out var name))
        {
            whereParts.Add("name ILIKE @nameFilter");
            parameters.Add(new NpgsqlParameter("nameFilter",$"{name}%"));
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
            SELECT id, name
            FROM tags 
            {where}
            {order}
            LIMIT @limit OFFSET @offset";

        string sqlCount = $@"
            SELECT COUNT(*)
            FROM tags
            {where}";

        await using var conn = await RepositoryHandler.OpenConnection();

        long totalElements;
        await using (var countCmd = new NpgsqlCommand(sqlCount, conn))
        {
            foreach (var parameter in parameters)
                countCmd.Parameters.Add(parameter.Clone());

            totalElements = Convert.ToInt64(await countCmd.ExecuteScalarAsync());
        }

        var listElements = new List<TagModelList>();
        await using (var dataCmd = new NpgsqlCommand(sqlData, conn))
        {
            foreach (var parameter in parameters)
                dataCmd.Parameters.Add(parameter.Clone());

            dataCmd.Parameters.AddWithValue("limit", page.Limit);
            dataCmd.Parameters.AddWithValue("offset", page.Limit * (page.Page - 1));

            await using var reader = await dataCmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                listElements.Add(TagsRepositoryReader.SerializeList(reader));
        }

        return new DAOList<TagModelList>(totalElements, listElements);
    }

    public static async Task<TagModel?> Get(long id)
    {
        const string sql = @"
            SELECT 
                id, 
                name,
                description
            FROM tags
            WHERE id = @id";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        var tag = TagsRepositoryReader.Serialize(reader);
        await reader.CloseAsync();
        return tag;
    }

    public static async Task<bool> Delete(long id)
    {
        const string sql = "DELETE FROM tags WHERE id = @id";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }

    public static async Task<long?> Insert(Tag tag) 
    {
        const string sql = @"
            INSERT INTO tags
                (name, description)
            VALUES
                (@name, @description)
            RETURNING id";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("name", tag.Name);
        cmd.Parameters.AddWithValue("description", tag.Description is null ? DBNull.Value : tag.Description);

        var result = await cmd.ExecuteScalarAsync();
        if (result is null || result == DBNull.Value)
            return null;

        return Convert.ToInt64(result);
    }

    public static async Task<bool> Update(Tag tag)
    {
        const string sql = @"
            UPDATE tags
            SET
                name = @name,
                description = @description
            WHERE id = @id";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("id", tag.ID);
        cmd.Parameters.AddWithValue("name", tag.Name);
        cmd.Parameters.AddWithValue("description", tag.Description is null ? DBNull.Value : tag.Description);

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }
    */
}