namespace DAO {

    public class DAOIndexCreator {

        public static async Task TagsName() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE INDEX IF NOT EXISTS idx_tags_name
                ON Tags(name);");

        public static async Task CategoriesName() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE INDEX IF NOT EXISTS idx_categories_name
                ON Categories(name);");

        public static async Task MonthlyServicesName() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE INDEX IF NOT EXISTS idx_monhtlyservices_name
                ON MonthlyServices(name);");

        public static async Task MonthlyServicesActive() =>
            await DAOUtils.CreateTableOrIndex(@$"
                CREATE INDEX IF NOT EXISTS idx_monhtlyservices_active
                ON MonthlyServices(isActive);");

    }

}