using Npgsql;

namespace Models {

    public class ModelTableCreator {

        public static async Task country_codes() {

            await using var conn = new NpgsqlConnection(ModelsManager.connection_string);
            await conn.OpenAsync();

            var table_to_be_created = @"
                CREATE TABLE IF NOT EXISTS CountryCodes (
                    id CHAR(3) PRIMARY KEY,
                    name VARCHAR(20) NOT NULL
                );
            ";

            await using var cmd = new NpgsqlCommand(table_to_be_created, conn);
            await cmd.ExecuteNonQueryAsync();
        }

        public static async Task bus_passes() {

            await using var conn = new NpgsqlConnection(ModelsManager.connection_string);
            await conn.OpenAsync();

            var table_to_be_created = @"
                CREATE TABLE IF NOT EXISTS BusPasses (
                  id CHAR(3) PRIMARY KEY,
                  discount NUMERIC(5,2) NOT NULL,
                  localityLevel SMALLINT NOT NULL,
                  duration INT NOT NULL,
                  active BOOLEAN NOT NULL
                );
            ";

            await using var cmd = new NpgsqlCommand(table_to_be_created, conn);
            await cmd.ExecuteNonQueryAsync();
        }

        public static async Task users() {

            await using var conn = new NpgsqlConnection(ModelsManager.connection_string);
            await conn.OpenAsync();

            var table_to_be_created = @"
                CREATE TABLE IF NOT EXISTS Users (

                    id VARCHAR(15) PRIMARY KEY,
                    password CHAR(64) NOT NULL,
                    salt INT NOT NULL,

                    name VARCHAR(50) NULL,
                    level CHAR(1) NOT NULL,
                    adminSinceDate DATE NULL,
                    inactiveDate DATE NULL,
                    inactiveAccountUser VARCHAR(15) NULL,
                    email VARCHAR(30) NULL,
                    birthDate DATE NULL,
                    sex CHAR(1) NOT NULL,
                    countryCode CHAR(3) NULL,
                    accountCreation TIMESTAMP NOT NULL,
                    public BOOLEAN NOT NULL,
                    disablePerson BOOLEAN NULL,
                    busPassID CHAR(3) NULL,
                    busPassValidFrom DATE NULL,
                    busPassValidUntil DATE NULL,

                    CONSTRAINT fk_inactiveAccountUser FOREIGN KEY (inactiveAccountUser)
                        REFERENCES Users(id)
                        ON DELETE SET NULL,
                    CONSTRAINT fk_countryCode FOREIGN KEY (countryCode)
                        REFERENCES CountryCodes(id)
                        ON DELETE SET NULL,
                    CONSTRAINT fk_busPass FOREIGN KEY (busPassID)
                        REFERENCES BusPasses(id)
                        ON DELETE SET NULL

                    );
            ";

            await using var cmd = new NpgsqlCommand(table_to_be_created, conn);
            await cmd.ExecuteNonQueryAsync();
        }

        public static async Task users_view() {

            await using var conn = new NpgsqlConnection(ModelsManager.connection_string);
            await conn.OpenAsync();

            string view_to_be_created = @"
                CREATE OR REPLACE VIEW VUsers AS SELECT                                                                                                                                                                               
                    u.id,
                    u.password,
                    u.salt,                                                                                  
                    u.name,
                    u.level,
                    u.adminSinceDate,
                    u.inactiveDate,
                    u.inactiveAccountUser,
                    u.email,
                    u.birthDate,
                    u.sex,
                    u.countryCode,
                    cc.name AS countryCodeName,
                    u.accountCreation,
                    u.public,
                    u.disablePerson,
                    u.busPassID,
                    u.busPassValidFrom,
                    u.busPassValidUntil,
                    bp.discount AS busPassDiscount,
                    bp.localityLevel AS busPassLocalityLevel,
                    bp.duration AS busPassDuration,
                    bp.active AS busPassActive
                FROM Users u
                LEFT JOIN CountryCodes cc ON u.countryCode = cc.id
                LEFT JOIN BusPasses bp ON u.busPassID = bp.id;
            ";

            await using var cmd = new NpgsqlCommand(view_to_be_created, conn);
            await cmd.ExecuteNonQueryAsync();

        }

        public static async Task token_view() {

            await using var conn = new NpgsqlConnection(ModelsManager.connection_string);
            await conn.OpenAsync();

            string view_to_be_created = @"
                CREATE OR REPLACE VIEW VAuthUsers                                                                                                                                                                               
                    AS SELECT id, password, salt, level, inactiveDate FROM Users;
            ";

            await using var cmd = new NpgsqlCommand(view_to_be_created, conn);
            await cmd.ExecuteNonQueryAsync();

        }

    }

}