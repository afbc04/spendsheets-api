namespace DAO {

    public class DAOViewCreator {

        public static async Task Collections() =>
            await DAOUtils.ExecQuery(@$"
                CREATE OR REPLACE VIEW VCollections AS
                  SELECT
                    co.id,                 
                    co.name,              
                    co.description,  
                    co.isMonthlyService,     
                    co.moneyamount,       
                    co.isMonthlyServiceActive,           
                    co.categoryrelated    AS categoryId,
                    ca.name                AS categoryName,
                    ca.description         AS categoryDescription
                  FROM Collections AS co
                  LEFT JOIN Categories AS ca
                    ON co.categoryrelated = ca.id;");

        public static async Task EntryDetails() =>
            await DAOUtils.ExecQuery(@$"
                CREATE OR REPLACE VIEW VEntries AS
                  SELECT
                    e.id,

                    e.categoryId           AS categoryId,
                    ca.name                AS categoryName,
                    ca.description         AS categoryDescription,

                    e.collectionId          AS collectionId,
                    co.name                 AS collectionName,
                    co.description          AS collectionDescription,
                    co.isMonthlyService     AS collectionIsMonthlyService,

                    e.isVisible,
                    e.isPublic,
                    e.isActive,
                    e.type,

                    e.moneyAmount,
                    e.moneyAmountSpent,

                    e.date,
                    e.lastChangeDate,
                    e.creationDate,
                    e.finishDate,
                    e.dueDate,
                    e.description,
                    e.status,
                    e.lastStatus,
                    e.deletionDate

                  FROM Entries AS e

                  LEFT JOIN Categories AS ca
                    ON e.categoryId = ca.id

                  LEFT JOIN Collections AS co
                    ON e.collectionId = co.id;");


        public static async Task EntryListing() =>
            await DAOUtils.ExecQuery(@$"
                CREATE OR REPLACE VIEW VEntryList AS
                  SELECT
                    e.id,

                    e.categoryId           AS categoryId,
                    ca.name                AS categoryName,

                    e.collectionId          AS collectionId,
                    co.name                 AS collectionName,
                    co.isMonthlyService     AS collectionIsMonthlyService,

                    e.isVisible,
                    e.isPublic,
                    e.isActive,
                    e.type,

                    e.moneyAmount,
                    e.moneyAmountSpent,

                    e.date,
                    e.lastChangeDate,
                    e.dueDate,
                    e.status

                  FROM Entries AS e

                  LEFT JOIN Categories AS ca
                    ON e.categoryId = ca.id

                  LEFT JOIN Collections AS co
                    ON e.collectionId = co.id;");

        /*
        public static async Task EntryTagsByTag() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE OR REPLACE VIEW VEntryTagsByTag AS
                  SELECT
                    et.entryId,
                    et.tagId,
                    t.name,
                    t.description

                  FROM EntryTags AS et

                  LEFT JOIN Tags AS t
                    ON et.tagId = t.id");*/


    }

}