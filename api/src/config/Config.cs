using System.Text.Json;
using Serilog;
using DAO;

namespace ConfigHandler {

    public class Config {

        public long database_version {private set; get;}
        public DateTime last_online_date {private set; get;}
        public string? name {private set; get;}
        public long money_initial {private set; get;}
        public long money_lost {private set; get;}
        public long money_saved {private set; get;}
        public bool is_public {private set; get;}

        private static Config? _config = null;
        private static readonly string config_path = "src/config/config.json";

        private Config() {

            this.database_version = DAOManager.DatabaseVersion;
            this.last_online_date = DateTime.UtcNow;
            this.name = null;
            this.money_initial = 0;
            this.money_lost = 0;
            this.money_saved = 0;
            this.is_public = false;

        }

        private Config(long database_version, DateTime last_online_date, string? name, bool is_public, long money_initial, long money_lost, long money_saved) {

            this.database_version = database_version;
            this.last_online_date = last_online_date;
            this.name = name;
            this.money_initial = money_initial;
            this.money_lost = money_lost;
            this.money_saved = money_saved;
            this.is_public = is_public;

        }

        public static Config Get() {

            if (Config._config == null)
                Config._config = Config.Load();

            return Config._config;

        }

        public static bool Update(ConfigUpdating config_updating) {

            Config new_config = new Config(
                Config._config!.database_version,
                Config._config!.last_online_date,
                config_updating.name,
                config_updating.is_public,
                config_updating.money_initial,
                config_updating.money_lost,
                config_updating.money_saved
            );

            Config last_config = Config._config;
            Config._config = new_config;
            bool was_saved = Config.Save();

            Config._config = was_saved ? new_config : last_config;
            return was_saved;

        }

        public void UpdateInstance() {

            this.database_version = DAOManager.DatabaseVersion;
            this.last_online_date = DateTime.UtcNow;

        }

        private static Config Load() {

            if (File.Exists(config_path)) {

                string json = File.ReadAllText(config_path);
                var json_dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json) ?? new Dictionary<string, JsonElement>();
                Log.Information("Config loaded");

                return new Config(
                    json_dict["database"].GetInt64(),
                    json_dict["lastOnlineDate"].GetDateTime(),
                    json_dict["name"].GetString(),
                    json_dict["public"].GetBoolean(),
                    json_dict["initialMoney"].GetInt64(),
                    json_dict["lostMoney"].GetInt64(),
                    json_dict["savedMoney"].GetInt64()
                );

            }
            else {
                Log.Information("Config created");
                return new Config();
            }

        }

        public static bool Save() {

            if (Config._config == null)
                return false;

            try {

                var config = Config._config;

                var json_obj = new Dictionary<string, object?>
                {
                    ["database"] = config.database_version,
                    ["lastOnlineDate"] = config.last_online_date.ToString("yyyy-MM-ddTHH:mm:ss"),
                    ["name"] = config.name,
                    ["public"] = config.is_public,
                    ["initialMoney"] = config.money_initial,
                    ["lostMoney"] = config.money_lost,
                    ["savedMoney"] = config.money_saved
                };

                var options = new JsonSerializerOptions{ WriteIndented = true };
                string json = JsonSerializer.Serialize(json_obj, options);

                File.WriteAllText(config_path, json);
                return true;

            }
            catch (Exception) {
                return false;
            }
        
        }

    }

}
