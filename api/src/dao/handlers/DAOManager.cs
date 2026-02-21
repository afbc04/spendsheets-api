using ConfigHandler;

namespace DAO {

    public class DAOManager {

        public static readonly int DatabaseVersion = 1;

        public static readonly string connection_string = $@"
            Host=database;
            Database={Environment.GetEnvironmentVariable("POSTGRES_DB")};
            Username={Environment.GetEnvironmentVariable("POSTGRES_USER")};
            Password={Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")}";

        public static async Task StartDatabase() {

            await DAOExtensionsCreator.Unaccent();

            await DAOTableCreator.Categories();
            
            await DAOTableCreator.Collections();
            await DAOViewCreator.Collections();
            
            await DAOTableCreator.Entry();
            await DAOTableCreator.EntryNotes();
            
            await DAOViewCreator.EntryDetails();
            await DAOViewCreator.EntryListing();

            // See if is needed 
            var config = Config.Get();

            if (config.database_version < DatabaseVersion)
                DAOUpdate.Upgrade(config.database_version);

        }

    }

}