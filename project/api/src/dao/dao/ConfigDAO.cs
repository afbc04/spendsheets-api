using Npgsql;
using Queries;

namespace Models {

    public class ConfigDAO {

        private static Config _serialize(NpgsqlDataReader reader) {
            return new Config(
                reader.GetInt32(1),
                reader.GetDateTime(2),
                reader.GetString(3), 
                (byte[])reader["password"],
                (byte[])reader["passwordSalt"],
                ModelUtil.get_string(reader,6),
                reader.GetBoolean(7),
                ModelUtil.get_money(reader,8),
                ModelUtil.get_money(reader,9),
                ModelUtil.get_money(reader,10)
                );
        }

        public async Task<Config?> get() {

            string sql = @"SELECT 
                    id, databaseVersion, lastOnlineDate, username, password, passwordSalt,
                    nameOfUser, isPublic, initMoney, lostMoney, savedMoney
                FROM Config WHERE id = 0;";
            return await ModelUtil.execute_query(sql, async cmd => {

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                    return _serialize(reader);

                return null;

            });

        }

        public async Task<bool> contains() {

            string sql = "SELECT COUNT(*) FROM Config WHERE id = 0;";
            return await ModelUtil.execute_query(sql, async cmd => {

                var count = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                return count > 0;

            });

        }

        public async Task<bool> put(Config config) {

            try {

                string sql = @"
                    INSERT INTO Config 
                        (id, databaseVersion, lastOnlineDate, username, password, passwordSalt,
                        nameOfUser, isPublic, initMoney, lostMoney, savedMoney)
                    VALUES
                        (0, @databaseVersion, @lastOnlineDate, @username, @password, @passwordSalt,
                        @nameOfUser, @isPublic, @initMoney, @lostMoney, @savedMoney)
                    ON CONFLICT (id) DO UPDATE
                    SET
                        databaseVersion = EXCLUDED.databaseVersion,
                        lastOnlineDate = EXCLUDED.lastOnlineDate,
                        password = EXCLUDED.password,
                        passwordSalt = EXCLUDED.passwordSalt,
                        nameOfUser = EXCLUDED.nameOfUser,
                        isPublic = EXCLUDED.isPublic,
                        initMoney = EXCLUDED.initMoney,
                        lostMoney = EXCLUDED.lostMoney,
                        savedMoney = EXCLUDED.savedMoney;";

                return await ModelUtil.execute_query(sql, async cmd => {
                    
                    cmd.Parameters.AddWithValue("@databaseVersion", config.database_version);
                    cmd.Parameters.AddWithValue("@lastOnlineDate", config.last_online_date);
                    cmd.Parameters.AddWithValue("@username", config.username);
                    cmd.Parameters.Add("@password", NpgsqlTypes.NpgsqlDbType.Bytea).Value = config._password ?? Array.Empty<byte>();
                    cmd.Parameters.Add("@passwordSalt", NpgsqlTypes.NpgsqlDbType.Bytea).Value = config._salt ?? Array.Empty<byte>();
                    cmd.Parameters.AddWithValue("@nameOfUser", (object?) config.name ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@isPublic", config.is_visible_to_public);
                    cmd.Parameters.AddWithValue("@initMoney", Convert.ToInt64(config.initial_money));
                    cmd.Parameters.AddWithValue("@lostMoney", Convert.ToInt64(config.lost_money));
                    cmd.Parameters.AddWithValue("@savedMoney", Convert.ToInt64(config.saved_money));
                    
                    await cmd.ExecuteNonQueryAsync();
                    return true;

                });

            }
            catch (Exception ex) {
                Console.WriteLine(ex.StackTrace);
                return false;
            }

        }

    }
}
