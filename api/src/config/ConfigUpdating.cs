namespace ConfigHandler {

    public class ConfigUpdating {

        public string? name { set; get;}
        public bool is_public {set; get;}
        public long money_initial { set; get;}
        public long money_lost { set; get;}
        public long money_saved { set; get;}

        public ConfigUpdating() {

            this.name = null;
            this.money_initial = 0;
            this.money_lost = 0;
            this.money_saved = 0;
            this.is_public = false;

        }

        public ConfigUpdating(string? name, bool is_public, long money_initial, long money_lost, long money_saved) {

            this.name = name;
            this.money_initial = money_initial;
            this.money_lost = money_lost;
            this.money_saved = money_saved;
            this.is_public = is_public;

        }

        public ConfigUpdating(Config config) {
            this.name = config.name;
            this.money_initial = config.money_initial;
            this.money_lost = config.money_lost;
            this.money_saved = config.money_saved;
            this.is_public = config.is_public;
        }

    }

}
