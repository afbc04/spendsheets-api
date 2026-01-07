namespace DAO {

    public class DAOTableCreator {

        public static async Task Config() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE TABLE IF NOT EXISTS Config (
                  id INT PRIMARY KEY,
                  databaseVersion INT NOT NULL,
                  lastOnlineDate TIMESTAMP NOT NULL,

                  username VARCHAR({ConfigRules.username_length_max}) NOT NULL,
                  password BYTEA NOT NULL,
                  passwordSalt BYTEA NOT NULL,

                  nameOfUser VARCHAR({ConfigRules.name_length_max}),
                  isPublic BOOLEAN NOT NULL,
                  initMoney INTEGER NOT NULL,
                  lostMoney INTEGER NOT NULL,
                  savedMoney INTEGER NOT NULL
                );");

        public static async Task Tags() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE TABLE IF NOT EXISTS Tags (
                  id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
                  name VARCHAR({TagRules.name_length_max}) NOT NULL,
                  description VARCHAR({TagRules.description_length_max}) 
                );");

        public static async Task Categories() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE TABLE IF NOT EXISTS Categories (
                  id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
                  name VARCHAR({CategoryRules.name_length_max}) NOT NULL,
                  description VARCHAR({CategoryRules.description_length_max}) 
                );");

        public static async Task MonthlyServices() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE TABLE IF NOT EXISTS MonthlyServices (
                  id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
                  name VARCHAR({MonthlyServiceRules.name_length_max}) NOT NULL,
                  description VARCHAR({MonthlyServiceRules.description_length_max}),
                  categoryRelated BIGINT,
                  moneyAmount INTEGER,
                  isActive BOOLEAN NOT NULL,
                CONSTRAINT fk_monthlyservices_category
                  FOREIGN KEY (categoryRelated)
                  REFERENCES Categories(id)
                  ON DELETE SET NULL
                );");

    }

}