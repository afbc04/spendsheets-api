namespace DAO {

    public class DAOViewCreator {

        public static async Task MonthlyServices() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE OR REPLACE VIEW VMonthlyServices AS
                  SELECT
                    ms.id,                 
                    ms.name,              
                    ms.description,       
                    ms.moneyamount,       
                    ms.isactive,           
                    ms.categoryrelated    AS categoryId,
                    c.name                AS categoryName,
                    c.description         AS categoryDescription
                  FROM MonthlyServices AS ms
                  LEFT JOIN Categories AS c
                    ON ms.categoryrelated = c.id;");

        public static async Task EntryDetails() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE OR REPLACE VIEW VEntries AS
                  SELECT
                    e.id,

                    e.categoryId          AS categoryId,
                    c.name                AS categoryName,
                    c.description         AS categoryDescription,

                    e.monthlyServiceId      AS monthlyServiceId,
                    ms.name                 AS monthlyServiceName,
                    ms.description          AS monthlyServiceDescription,
                    ms.isActive             AS monthlyServiceActive,

                    e.isVisible,
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
                    de.deletionDate,
                    de.deletedStatus,
                    de.lastStatus

                  FROM Entries AS e

                  LEFT JOIN Categories AS c
                    ON e.categoryId = c.id

                  LEFT JOIN MonthlyServices AS ms
                    ON e.monthlyServiceId = ms.id

                  LEFT JOIN DeletedEntries AS de
                    ON e.id = de.id;");

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
                    ON et.tagId = t.id");


    }

}