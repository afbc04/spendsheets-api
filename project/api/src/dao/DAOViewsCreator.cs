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

                    e.isVisible,
                    e.type,
                    e.moneyAmount,
                    e.moneyAmountLeft,
                    e.date,
                    e.lastChangeDate,
                    e.creationDate,
                    e.finishDate,
                    e.description,
                    e.status,
                    de.deletionDate,
                    de.deletedStatus,
                    de.lastStatus,

                    ce.monthlyServiceId     AS monthlyServiceId,
                    ms.name                 AS monthlyServiceName,
                    ms.description          AS monthlyServiceDescription,
                    ms.isActive             AS monthlyServiceActive,

                    ce.isGeneratedBySystem,
                    ce.scheduledDueDate,
                    ce.realDueDate,

                    COALESCE(et.tagsCount, 0) AS tagsCount

                  FROM Entries AS e

                  LEFT JOIN Categories AS c
                    ON e.categoryId = c.id

                  LEFT JOIN DeletedEntries AS de
                    ON e.id = de.id

                  LEFT JOIN CommitmentEntries AS ce
                    ON e.id = ce.id

                  LEFT JOIN MonthlyServices AS ms
                    ON ce.monthlyServiceId = ms.id
                    
                  LEFT JOIN (
                      SELECT entryId, COUNT(*) AS tagsCount
                      FROM EntryTags
                      GROUP BY entryId
                  ) et ON e.id = et.entryId;");

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