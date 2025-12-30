namespace DTO {

    public class ConfigDTOException : Exception {

        public string message {private set; get;}

        public ConfigDTOException(string message) {
            this.message = message;
        }

    }

    public class ConfigDTO {

        private Config _config;

        public ConfigDTO() {
            this._config = new Config("","");
        }

        public ConfigDTO(Config config) {
            this._config = config;
        }

        public Config extract() {
            return this._config;
        }

        public void set_username(string username) {

            if (username.Length < Config.username_length_min)
                throw new ConfigDTOException($"Username is too short (less than {Config.username_length_min} characters)");

            if (username.Length >= Config.username_length_max)
                throw new ConfigDTOException($"Username is too long (more than {Config.username_length_max} characters)");

            this._config.username = username;

        }

        public void set_password(string password) {

            if (password.Length < Config.password_length_min)
                throw new ConfigDTOException($"Password is too short (less than {Config.password_length_min} characters)");

            if (password.Length >= Config.password_length_max)
                throw new ConfigDTOException($"Password is too long (more than {Config.password_length_max} characters)");

            this._config.set_password(password);

        }

        public void set_name(string name) {

            if (name.Length >= Config.name_length_max)
                throw new ConfigDTOException($"Name is too long (more than {Config.name_length_max} characters)");

            this._config.name = name;

        }

        public void set_public(bool is_public) {
            this._config.is_visible_to_public = is_public;
        }

        public void set_initial_money(long initial_money) {

            if (initial_money < 0)
                throw new ConfigDTOException("Initial money can not be negative");

            this._config.initial_money = (uint) initial_money;

        }

        public void set_lost_money(long lost_money) {

            if (lost_money < 0)
                throw new ConfigDTOException("Lost money can not be negative");

            this._config.lost_money = (uint) lost_money;

        }

        public void set_saved_money(long saved_money) {

            if (saved_money < 0)
                throw new ConfigDTOException("Saved money can not be negative");

            this._config.saved_money = (uint) saved_money;

        }

    }


}