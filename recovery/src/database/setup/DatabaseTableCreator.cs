
public class DatabaseTableCreator
{
  /*
  public static async Task<int> DatabaseConfigurations() =>
    await DatabaseUtils.ExecuteQuery(@$"
      CREATE TABLE IF NOT EXISTS database_config (
        id SMALLINT PRIMARY KEY,
        database_version BIGINT NOT NULL
      );");

  public static async Task<int> Users() =>
    await DatabaseUtils.ExecuteQuery(@$"
      CREATE TABLE IF NOT EXISTS users (
        id SMALLINT PRIMARY KEY,

        username VARCHAR({UserRules.UsernameLengthMax}) NOT NULL,
        name VARCHAR({UserRules.NameLengthMax}),

        initial_money BIGINT NOT NULL,
        creation_date DATE NOT NULL,

        password_hash BYTEA NOT NULL,
        password_salt BYTEA NOT NULL
      );");

  public static async Task<int> Categories() => await DatabaseUtils.ExecuteQuery(@$"
    CREATE TABLE IF NOT EXISTS categories (
      id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
      name VARCHAR({CategoryRules.NameLengthMax}) NOT NULL,
      description VARCHAR({CategoryRules.DescriptionLengthMax}),
      parent_id BIGINT NULL,
      created_at DATE NOT NULL DEFAULT CURRENT_DATE,

      CONSTRAINT fk_categories_parent
        FOREIGN KEY (parent_id)
        REFERENCES Categories(id)
        ON DELETE SET NULL
    );");

  public static async Task<int> Tags() => await DatabaseUtils.ExecuteQuery(@$"
    CREATE TABLE IF NOT EXISTS tags (
      id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
      name VARCHAR({TagRules.NameLengthMax}) NOT NULL,
      description VARCHAR({TagRules.DescriptionLengthMax})
    );");

  public static async Task<int> Records() => await DatabaseUtils.ExecuteQuery(@$"
    CREATE TABLE IF NOT EXISTS records (
        id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1),
        description VARCHAR({RecordRules.DescriptionLengthMax}),
        date DATE NOT NULL,
        money_total BIGINT NOT NULL,

        invisible BOOLEAN NOT NULL DEFAULT FALSE,
        public BOOLEAN NOT NULL DEFAULT FALSE,

        createdAt DATE NOT NULL,
        updatedAt DATE NOT NULL,
        deletedAt DATE,

        status SMALLINT NOT NULL
    );");

  public static async Task<int> RecordItems() => await DatabaseUtils.ExecuteQuery(@$"
    CREATE TABLE IF NOT EXISTS record_items (
        record_id BIGINT NOT NULL,
        record_item_index SMALLINT NOT NULL,

        money INT NOT NULL,
        note VARCHAR({RecordItemRules.NoteLengthMax}),

        category_id BIGINT,

        PRIMARY KEY(record_id, record_item_index),

        FOREIGN KEY(record_id) REFERENCES records(id),
        FOREIGN KEY(category_id) REFERENCES categories(id)
    );");

  public static async Task<int> RecordItemsTags() => await DatabaseUtils.ExecuteQuery(@$"
    CREATE TABLE IF NOT EXISTS record_items_tags (
        record_id BIGINT NOT NULL,
        record_item_index SMALLINT NOT NULL,
        tag_id BIGINT NOT NULL,

        PRIMARY KEY(record_id, record_item_index, tag_id),

        FOREIGN KEY(record_id, record_item_index) REFERENCES record_items(record_id, record_item_index),
        FOREIGN KEY(tag_id) REFERENCES tags(id)
    );
    
");

  /*
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
                  );");*/

}
