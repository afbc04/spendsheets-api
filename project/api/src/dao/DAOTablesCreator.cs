namespace DAO {

    public class DAOTableCreator {

        public static async Task Config() =>
            await DAOUtils.CreateTable(@$"
                CREATE TABLE IF NOT EXISTS Config (
                  id INT PRIMARY KEY,
                  databaseVersion INT NOT NULL,
                  lastOnlineDate TIMESTAMP NOT NULL,

                  username VARCHAR({ConfigRules.username_length_max}) NOT NULL,
                  password BYTEA NOT NULL,
                  passwordSalt BYTEA NOT NULL,

                  nameOfUser VARCHAR({ConfigRules.name_length_max}),
                  isPublic BOOLEAN NOT NULL,
                  initMoney BIGINT NOT NULL,
                  lostMoney BIGINT NOT NULL,
                  savedMoney BIGINT NOT NULL
                );");

        public static async Task Tags() =>
            await DAOUtils.CreateTable(@$"
                CREATE TABLE IF NOT EXISTS Tags (
                  id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
                  name VARCHAR({TagRules.name_length_max}) NOT NULL,
                  description VARCHAR({TagRules.description_length_max}) 
                );");

    }

}