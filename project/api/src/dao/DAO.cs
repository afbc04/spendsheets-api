namespace DAO {

    public class DAOListing<T> {

        public long count { get; }
        public List<T> list { get; }

        public DAOListing(long count, List<T> list){
            this.count = count;
            this.list = list;
        }

    }


    public class DAOManager {

        public static readonly int database_version = 1;

        public static readonly string connection_string = $@"
            Host=database;
            Database={Environment.GetEnvironmentVariable("POSTGRES_DB")};
            Username={Environment.GetEnvironmentVariable("POSTGRES_USER")};
            Password={Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")}";

        public static async Task StartDatabase() {

            await DAOTableCreator.Config();
            
            await DAOTableCreator.Tags();
            await DAOIndexCreator.TagsName();

        }

    }

}