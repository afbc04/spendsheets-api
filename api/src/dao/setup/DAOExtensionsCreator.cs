namespace DAO {

    public class DAOExtensionsCreator {

        public static async Task Unaccent() =>
            await DAOUtils.ExecQuery(@$"
                CREATE EXTENSION IF NOT EXISTS unaccent;");

    }

}