using Npgsql;
using NpgsqlTypes;
using Serilog;

namespace DAO {

    enum ConfigField {
        id,
        databaseVersion,
        lastOnlineDate,
        username,
        password,
        passwordSalt,
        nameOfUser,
        isPublic,
        initMoney,
        lostMoney,
        savedMoney
    };

    public class ConfigDAO {

        private static Config _serialize(NpgsqlDataReader r) {
            return new Config(
                r.getInt((int) ConfigField.databaseVersion),
                r.getDateTime((int) ConfigField.lastOnlineDate),
                r.getString((int) ConfigField.username),
                r.getBytes((int) ConfigField.password),
                r.getBytes((int) ConfigField.passwordSalt),
                r.tryGetString((int) ConfigField.nameOfUser),
                r.getBool((int) ConfigField.isPublic),
                r.getLong((int) ConfigField.initMoney),
                r.getLong((int) ConfigField.lostMoney),
                r.getLong((int) ConfigField.savedMoney)
            );
        }

        public async Task<Config?> Get() {

            string sql = @"SELECT 
                    id, databaseVersion, lastOnlineDate, username, password, passwordSalt,
                    nameOfUser, isPublic, initMoney, lostMoney, savedMoney
                FROM Config WHERE id = 0;";
            return await DAOUtils.Query(sql, async cmd => {

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                    return _serialize(reader);

                return null;

            });

        }

        public async Task<bool> Delete() {

            const string sql = "DELETE FROM Config WHERE id = 0";
            return await DAOUtils.Query(sql, async cmd => {

                var lines = await cmd.ExecuteNonQueryAsync();
                return lines > 0;

            });

        }

        public async Task<bool> Put(Config config) {

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

                return await DAOUtils.Query(sql, async cmd => {
                    
                    cmd.Parameters.Add("@databaseVersion", NpgsqlDbType.Integer)
                        .Value = config.database_version;

                    cmd.Parameters.Add("@lastOnlineDate", NpgsqlDbType.TimestampTz)
                        .Value = config.last_online_date.ToUniversalTime();

                    cmd.Parameters.Add("@username", NpgsqlDbType.Varchar)
                        .Value = config.username;

                    cmd.Parameters.Add("@password", NpgsqlDbType.Bytea)
                        .Value = config._password;

                    cmd.Parameters.Add("@passwordSalt", NpgsqlDbType.Bytea)
                        .Value = config._salt;

                    cmd.Parameters.Add("@nameOfUser", NpgsqlDbType.Varchar)
                        .Value = (object?) config.name ?? DBNull.Value;

                    cmd.Parameters.Add("@isPublic", NpgsqlDbType.Boolean)
                        .Value = config.is_visible_to_public;

                    cmd.Parameters.Add("@initMoney", NpgsqlDbType.Bigint)
                        .Value = config.initial_money;

                    cmd.Parameters.Add("@lostMoney", NpgsqlDbType.Bigint)
                        .Value = config.lost_money;

                    cmd.Parameters.Add("@savedMoney", NpgsqlDbType.Bigint)
                        .Value = config.saved_money;
                    
                    await cmd.ExecuteNonQueryAsync();
                    return true;

                });

            }
            catch (Exception ex) {
                Log.Error(ex.StackTrace ?? ex.Message);
                return false;
            }

        }

    }
}
