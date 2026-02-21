using Controller;
using ConfigHandler;

namespace Manager {

    public class APIManager {

        public ConfigManager Config {private set; get;}
        public TokenManager Token {private set; get;}
        public CategoryManager Category {private set; get;}
        public CollectionManager Collection {private set; get;}
        public EntryManager Entry {private set; get;}
        public EntryNotesManager EntryNotes {private set; get;}

        private APIManager(ConfigController config, TokenController token, CategoryController category, CollectionController collection, EntryController entry, EntryNotesController entry_notes) {
            
            this.Config = new ConfigManager(config,token);
            this.Token = new TokenManager(token);
            this.Category = new CategoryManager(category,token);
            this.Collection = new CollectionManager(collection,category,token);
            this.Entry = new EntryManager(entry,collection,category,token);
            this.EntryNotes = new EntryNotesManager(entry_notes,entry,token);

        }

        public static async Task<APIManager> Start() {

            //Get config
            var config = ConfigHandler.Config.Get();

            //Create controllers
            var config_controller = new ConfigController();
            var token_controller = new TokenController(config.is_public);
            var category_controller = new CategoryController();
            var collection_controller = new CollectionController();
            var entry_controller = new EntryController();
            var entry_notes_controller = new EntryNotesController();

            // Load controllers
            /*
            await config_controller._Load();



            await config_controller._Finish();*/

            // Create manager
            return new APIManager(config_controller,token_controller,category_controller,collection_controller,entry_controller,entry_notes_controller);

        }

    }

}


