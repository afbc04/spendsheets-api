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

        public static async Task Entry() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE TABLE IF NOT EXISTS Entries (
                  id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
                  categoryId BIGINT,
                  isVisible BOOLEAN NOT NULL,
                  type CHAR(1) NOT NULL,
                  moneyAmount INTEGER NOT NULL,
                  moneyAmountLeft INTEGER,
                  lastChangeDate TIMESTAMP NOT NULL,
                  creationDate DATE NOT NULL,
                  finishDate DATE,
                  date DATE NOT NULL,
                  description VARCHAR({EntryRules.description_length_max}),
                  status CHAR(1) NOT NULL,
                CONSTRAINT fk_entries_category
                  FOREIGN KEY (categoryId)
                  REFERENCES Categories(id)
                  ON DELETE SET NULL
                );");

        public static async Task EntryDeleted() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE TABLE IF NOT EXISTS DeletedEntries (
                  id BIGINT PRIMARY KEY,
                  deletionDate DATE NOT NULL,
                  deletedStatus CHAR(1) NOT NULL,
                  lastStatus CHAR(1) NOT NULL,
                CONSTRAINT fk_deletedentries_entry
                  FOREIGN KEY (id)
                  REFERENCES Entries(id)
                  ON DELETE CASCADE
                );");

        public static async Task EntryCommitment() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE TABLE IF NOT EXISTS CommitmentEntries (
                  id BIGINT PRIMARY KEY,
                  monthlyServiceId BIGINT,
                  isGeneratedBySystem BOOLEAN NOT NULL,
                  scheduledDueDate DATE,
                  realDueDate DATE,
                CONSTRAINT fk_commitmententries_entry
                  FOREIGN KEY (id)
                  REFERENCES Entries(id)
                  ON DELETE CASCADE,
                CONSTRAINT fk_commitmententries_monthlyservice
                  FOREIGN KEY (monthlyServiceId)
                  REFERENCES MonthlyServices(id)
                  ON DELETE SET NULL
                );");

          public static async Task EntryTags() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE TABLE IF NOT EXISTS EntryTags (
                  entryId BIGINT NOT NULL,
                  tagId BIGINT NOT NULL,
                CONSTRAINT pk_entry_tags
                  PRIMARY KEY (entryId, tagId),
                CONSTRAINT fk_entrytag_entry
                  FOREIGN KEY (entryId)
                  REFERENCES Entries(id)
                  ON DELETE CASCADE,
                CONSTRAINT fk_entrytag_tag
                  FOREIGN KEY (tagId)
                  REFERENCES Tags(id)
                  ON DELETE CASCADE
                );");

    }

}