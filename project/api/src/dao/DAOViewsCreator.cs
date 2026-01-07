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

    }

}