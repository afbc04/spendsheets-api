namespace DAO {

    public class DAOTableCreator {

        public static async Task Categories() =>
            await DAOUtils.ExecQuery(@$"
                CREATE TABLE IF NOT EXISTS Categories (
                  id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
                  name VARCHAR({CategoryRules.name_length_max}) NOT NULL,
                  description VARCHAR({CategoryRules.description_length_max}) 
                );");

        public static async Task Collections() =>
            await DAOUtils.ExecQuery(@$"
                CREATE TABLE IF NOT EXISTS Collections (
                  id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
                  name VARCHAR({CollectionRules.name_length_max}) NOT NULL,
                  description VARCHAR({CollectionRules.description_length_max}),
                  isMonthlyService BOOLEAN NOT NULL,
                  categoryRelated BIGINT,
                  moneyAmount INTEGER,
                  isMonthlyServiceActive BOOLEAN,
                CONSTRAINT fk_collection_category
                  FOREIGN KEY (categoryRelated)
                  REFERENCES Categories(id)
                  ON DELETE SET NULL
                );");

        public static async Task Entry() =>
            await DAOUtils.ExecQuery(@$"
                CREATE TABLE IF NOT EXISTS Entries (
                  id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
                  categoryId BIGINT,
                  collectionId BIGINT,
                  isVisible BOOLEAN NOT NULL,
                  isPublic BOOLEAN NOT NULL,
                  isActive BOOLEAN NOT NULL,
                  type CHAR(1) NOT NULL,
                  moneyAmount INTEGER NOT NULL,
                  moneyAmountSpent INTEGER,
                  lastChangeDate TIMESTAMP NOT NULL,
                  creationDate DATE NOT NULL,
                  finishDate DATE,
                  date DATE NOT NULL,
                  dueDate DATE,
                  description VARCHAR({EntryRules.description_length_max}),
                  status CHAR(1) NOT NULL,
                  lastStatus CHAR(1),
                  deletionDate DATE,
                CONSTRAINT fk_entries_category
                  FOREIGN KEY (categoryId)
                  REFERENCES Categories(id)
                  ON DELETE SET NULL,
                CONSTRAINT fk_entries_collection
                  FOREIGN KEY (collectionId)
                  REFERENCES Collections(id)
                  ON DELETE SET NULL
                );");

        public static async Task EntryNotes() =>
            await DAOUtils.ExecQuery(@$"
                CREATE TABLE IF NOT EXISTS EntryNotes (
                  id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
                  entryId BIGINT NOT NULL,
                  money INTEGER,
                  note VARCHAR({EntryRules.note_length_max}),
                  date DATE NOT NULL,
                CONSTRAINT fk_entrynotes_entry
                  FOREIGN KEY (entryId)
                  REFERENCES Entries(id)
                  ON DELETE CASCADE
                );");

    }

}