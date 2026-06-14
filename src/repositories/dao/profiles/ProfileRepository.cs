using Npgsql;

public static class ProfileRepository
{
    public static async Task<List<Profile>> List()
    {
        const string sql = @"
            SELECT
                username,
                name,
                creation_date,
                is_admin,
                inactive_date,
                password_hash,
                password_salt
            FROM profiles";

        var profiles = new List<Profile>();

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            profiles.Add(ProfileRepositoryReader.Serialize(reader));

        return profiles;
    }

    public static async Task<bool> Delete(string username)
    {
        const string sql = @"DELETE FROM profiles WHERE username = @username;";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("username", username);

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }

    public static async Task<Profile?> Get(string username)
    {
        const string sql = @"
            SELECT 
                username,
                name,
                creation_date,
                is_admin,
                inactive_date,
                password_hash,
                password_salt
            FROM profiles
            WHERE username = @username";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("username", username);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        var profile = ProfileRepositoryReader.Serialize(reader);
        await reader.CloseAsync();
        return profile;
    }

    public static async Task<bool> Put(Profile profile)
    {
        const string sql = @"
            INSERT INTO profiles
                (username, name, creation_date, is_admin, inactive_date, password_hash, password_salt)
            VALUES
                (@username, @name, @creationDate, @isAdmin, @inactiveDate, @passwordHash, @passwordSalt)
            ON CONFLICT (username)
            DO UPDATE SET
                name = EXCLUDED.name,
                is_admin = EXCLUDED.is_admin,
                inactive_date = EXCLUDED.inactive_date,
                password_hash = EXCLUDED.password_hash,
                password_salt = EXCLUDED.password_salt;";

        await using var conn = await RepositoryHandler.OpenConnection();
        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("username", profile.Username);
        cmd.Parameters.AddWithValue("name", profile.Name);
        cmd.Parameters.AddWithValue("creationDate", profile.CreationDate);
        cmd.Parameters.AddWithValue("isAdmin", profile.IsAdmin);
        cmd.Parameters.AddWithValue("inactiveDate", profile.InactiveDate is null ? DBNull.Value : profile.InactiveDate);
        cmd.Parameters.AddWithValue("passwordHash", profile.PasswordHash);
        cmd.Parameters.AddWithValue("passwordSalt", profile.PasswordSalt);

        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }
}