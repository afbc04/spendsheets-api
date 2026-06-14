using Npgsql;
using Serilog;

public class DatabaseManager
{
    
    public static readonly int DatabaseVersion = 1; //Current expected version of database
    public static readonly int DatabaseVersionLastCompatible = 1; //Older compatible database version this API can serve

    public static readonly string ConnectionString = $@"
            Host=localhost;
            Database={Environment.GetEnvironmentVariable("POSTGRES_DB")};
            Username={Environment.GetEnvironmentVariable("POSTGRES_USER")};
            Password={Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")}";
    public static async Task<DatabaseStatus> LinkWithDatabase()
    {
        try
        {
            // 1. Verify if database can be connected
            using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            if (conn is null)
                return DatabaseStatus.ConnectionFail;

            // 2. Check if configuration exists
            const string checkConfigSql = @"
                SELECT EXISTS (
                    SELECT 1
                    FROM information_schema.tables
                    WHERE table_name = 'database_config'
                );";

            await using (var cmd = new NpgsqlCommand(checkConfigSql, conn))
            {
                var exists = (bool?) await cmd.ExecuteScalarAsync();
                if (exists is null || !(bool) exists)
                {
                    return await DatabaseSetup(true);
                }
            }

            // 3. Checks which version database is
            const string versionSql = @"
                SELECT database_version
                FROM database_config
                WHERE id = 0;";

            await using (var cmd = new NpgsqlCommand(versionSql, conn))
            {
                var result = await cmd.ExecuteScalarAsync();
                if (result == null || result == DBNull.Value)
                    return DatabaseStatus.Corrupted;

                // 4. Verify version of existing database
                long version = Convert.ToInt64(result);
                await DatabaseSetup(false);

                if (version < DatabaseVersionLastCompatible)
                    return DatabaseStatus.OlderVersion;

                if (version > DatabaseVersion)
                    return DatabaseStatus.NewerVersion;

                if (version < DatabaseVersion)
                {
                    // TODO: upgrade
                }

                return DatabaseStatus.Success;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            return DatabaseStatus.Exception;
        }
    }

    private static async Task<DatabaseStatus> DatabaseSetup(bool shouldCreateDatabase)
    {
        int res = 1;

/*
        res *= await DatabaseTableCreator.DatabaseConfigurations();

        if (shouldCreateDatabase)
            res *= await DatabaseUtils.ExecuteQuery($@"
                INSERT INTO database_config (id, database_version)
                    VALUES (0, {DatabaseVersion});
            ");*/

        res *= await DatabaseTableCreator.Profiles();
/*
        res *= await DatabaseTableCreator.Users();
        res *= await DatabaseTableCreator.Categories();
        res *= await DatabaseTableCreator.Tags();

        res *= await DatabaseTableCreator.Records();
        res *= await DatabaseTableCreator.RecordItems();
        res *= await DatabaseTableCreator.RecordItemsTags();

        */

        return res == 0
            ? DatabaseStatus.SetupFail
            : DatabaseStatus.Success;
    }
}