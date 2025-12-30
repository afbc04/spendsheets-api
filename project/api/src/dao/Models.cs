namespace Models {

    public class ModelListing<T> {

        public long all_elements {get; set;}
        public IList<T> list {get; set;}

        public ModelListing(long all_elements, IList<T> list) {
            this.all_elements = all_elements;
            this.list = list;
        }

    }

    public class ModelsManager {

        public static readonly string connection_string = $@"
            Host=database;
            Database={Environment.GetEnvironmentVariable("POSTGRES_DB")};
            Username={Environment.GetEnvironmentVariable("POSTGRES_USER")};
            Password={Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")}";

        public static async Task init() {

            await ModelTableCreator.config();
            //await ModelTableCreator.bus_passes();
            //await ModelTableCreator.users();
            //await ModelTableCreator.users_view();
            //await ModelTableCreator.token_view();

        }

    }

}