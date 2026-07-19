using Npgsql;

public static class WorkspaceRepository
{
    public static async Task<List<Workspace>> List()
    {
        const string sql = $@"
            SELECT 
                id, 
                name, 
                description, 
                initial_money
            FROM workspaces";

        var workspaces = new List<Workspace>();

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            workspaces.Add(WorkspaceRepositoryReader.Serialize(reader));

        return workspaces;
    }

    public static async Task<bool> Delete(long id)
    {
        const string sql = "DELETE FROM workspaces WHERE id = @id";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }

    public static async Task<Workspace?> Get(long id)
    {
        const string sql = @"
            SELECT 
                id, 
                name, 
                description, 
                initial_money
            FROM workspaces
            WHERE id = @id";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        var workspace = WorkspaceRepositoryReader.Serialize(reader);
        await reader.CloseAsync();
        return workspace;
    }

    public static async Task<long?> Insert(Workspace workspace)
    {
        const string sql = @"
            INSERT INTO workspaces
                (name, description, initial_money)
            VALUES
                (@name, @description, @initialMoney)
            RETURNING id;";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("name", workspace.Name);
        cmd.Parameters.AddWithValue("description", workspace.Description is null ? DBNull.Value : workspace.Description);
        cmd.Parameters.AddWithValue("initialMoney", workspace.InitialMoney);

        var result = await cmd.ExecuteScalarAsync();
        if (result is null || result == DBNull.Value)
            return null;

        return Convert.ToInt64(result);
    }

    public static async Task<bool> Update(Workspace workspace)
    {
        const string sql = @"
            UPDATE workspaces
            SET
                name = @name,
                description = @description,
                initial_money = @initialMoney
            WHERE id = @id;";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("id", workspace.ID);
        cmd.Parameters.AddWithValue("name", workspace.Name);
        cmd.Parameters.AddWithValue("description", workspace.Description is null ? DBNull.Value : workspace.Description);
        cmd.Parameters.AddWithValue("initialMoney", workspace.InitialMoney);

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }
}