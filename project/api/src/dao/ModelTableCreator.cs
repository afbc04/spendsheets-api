using Npgsql;

namespace Models {

    public class ModelTableCreator {

        public static async Task config() {

            await using var conn = new NpgsqlConnection(ModelsManager.connection_string);
            await conn.OpenAsync();

            var table_to_be_created = @"
                CREATE TABLE IF NOT EXISTS Config (
                  id INT PRIMARY KEY,
                  databaseVersion INT NOT NULL,
                  lastOnlineDate TIMESTAMP NOT NULL,

                  username VARCHAR(30) NOT NULL,
                  password BYTEA NOT NULL,
                  passwordSalt BYTEA NOT NULL,

                  nameOfUser VARCHAR(64),
                  isPublic BOOLEAN NOT NULL,
                  initMoney BIGINT NOT NULL,
                  lostMoney BIGINT NOT NULL,
                  savedMoney BIGINT NOT NULL
                );";

            await using var cmd = new NpgsqlCommand(table_to_be_created, conn);
            await cmd.ExecuteNonQueryAsync();
        }

    }

}