using Npgsql;

public static class UserRepository
{
    /*
    public static async Task<User?> Get()
    {
        const string sql = @"
            SELECT 
                username,
                name,
                initial_money,
                creation_date,
                password_hash,
                password_salt
            FROM users
            WHERE id = 0";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        var user = UserRepositoryReader.Serialize(reader);
        await reader.CloseAsync();
        return user;
    }

    public static async Task<bool> Insert(User user) 
    {
        const string sql = @"
            INSERT INTO users
                (id, username, name, initial_money, creation_date, password_hash, password_salt)
            VALUES
                (0, @username, @name, @initialMoney, @creationDate, @passwordHash, @passwordSalt)";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("username", user.Username);
        cmd.Parameters.AddWithValue("name", user.Name is null ? DBNull.Value : user.Name);
        cmd.Parameters.AddWithValue("initialMoney", user.InitialMoney);
        cmd.Parameters.AddWithValue("creationDate", user.CreationDate);
        cmd.Parameters.AddWithValue("passwordHash", user.PasswordHash);
        cmd.Parameters.AddWithValue("passwordSalt", user.PasswordSalt);

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }

    public static async Task<bool> Update(User user)
    {
        const string sql = @"
            UPDATE users
            SET
                username = @username,
                name = @name,
                initial_money = @initialMoney,
                password_hash = @passwordHash,
                password_salt = @passwordSalt
            WHERE id = 0";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("username", user.Username);
        cmd.Parameters.AddWithValue("name", user.Name is null ? DBNull.Value : user.Name);
        cmd.Parameters.AddWithValue("initialMoney", user.InitialMoney);
        cmd.Parameters.AddWithValue("passwordHash", user.PasswordHash);
        cmd.Parameters.AddWithValue("passwordSalt", user.PasswordSalt);

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }*/
}