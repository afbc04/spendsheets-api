namespace DTO {

    // Exception when some proprieties break rules
    public class ConfigDTOException : Exception {

        public string message {private set; get;}

        public ConfigDTOException(string message) {
            this.message = message;
        }

    }

    public class ConfigDTO {

        private Config _config;

        public ConfigDTO() {
            this._config = new Config("",[]);
        }

        public ConfigDTO(Config config) {
            this._config = config;
        }

        // Final method
        public Config extract() {
            return this._config;
        }

        // @@@@@@@@@@@@@@@@
        //    SETTERS
        // @@@@@@@@@@@@@@@@
        public void set_username(string username) {

            if (username.Length < ConfigRules.username_length_min)
                throw new ConfigDTOException($"Username is too short (less than {ConfigRules.username_length_min} characters)");

            if (username.Length >= ConfigRules.username_length_max)
                throw new ConfigDTOException($"Username is too long (more than {ConfigRules.username_length_max} characters)");

            this._config.username = username;

        }

        public void set_password(string password) {

            if (password.Length < ConfigRules.password_length_min)
                throw new ConfigDTOException($"Password is too short (less than {ConfigRules.password_length_min} characters)");

            if (password.Length >= ConfigRules.password_length_max)
                throw new ConfigDTOException($"Password is too long (more than {ConfigRules.password_length_max} characters)");

            this._config.set_password(password);

        }

        public void set_name(string? name) {

            if (name != null && name.Length >= ConfigRules.name_length_max)
                throw new ConfigDTOException($"Name is too long (more than {ConfigRules.name_length_max} characters)");

            this._config.name = name;

        }

        public void set_public(bool is_public) {
            this._config.is_visible_to_public = is_public;
        }

        public void set_initial_money(double initial_money) {

            if (initial_money < 0)
                throw new ConfigDTOException("Initial money can not be negative");

            this._config.initial_money = Money.Convert64(initial_money);

        }

        public void set_lost_money(double lost_money) {

            if (lost_money < 0)
                throw new ConfigDTOException("Lost money can not be negative");

            this._config.lost_money = Money.Convert64(lost_money);

        }

        public void set_saved_money(double saved_money) {

            if (saved_money < 0)
                throw new ConfigDTOException("Saved money can not be negative");

            this._config.saved_money = Money.Convert64(saved_money);

        }

    }


}