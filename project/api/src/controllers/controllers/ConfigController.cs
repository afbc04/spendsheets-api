using PacketHandlers;
using DAO;
using Nito.AsyncEx;
using DTO;

namespace Controller {

    public class ConfigController {

        public readonly AsyncReaderWriterLock Lock;
        private ConfigDAO dao;

        public Config? config {private set; get;}
        public DateTime last_online_date {private set; get;}
        public bool is_next_month {private set; get;}
        public bool outdated_database {private set; get;}
        private Config? config_to_be_updated;


        public ConfigController() {
            this.Lock = new();
            this.dao = new ConfigDAO();
        }

        // @@@@@@@@@@@@@@@@@@@@@@@@@
        //  Auxiliar Funcionalities
        // @@@@@@@@@@@@@@@@@@@@@@@@@
        public bool _ConfigExists() {
            return this.config != null;
        }

        public bool _IsPublic() {
            return this.config!.is_visible_to_public;
        }

        public async Task _Load() {

            Config? config = await this.dao.Get();

            if (config != null) {

                // Update entries
                DateTime date = DateTime.UtcNow;
                Console.WriteLine($"Tempo que passou {date - config.last_online_date}");
                this.last_online_date = config.last_online_date;
                this.is_next_month = date.Year > config.last_online_date.Year || date.Month > config.last_online_date.Month;
                config.last_online_date = date;

                // Update Database
                this.outdated_database = config.database_version < DAO.DAOManager.database_version;

                // Prepare new config
                this.config_to_be_updated = config;

            }

        }

        public async Task _Finish() {

            if (this.config_to_be_updated != null) {

                this.config = this.config_to_be_updated;
                bool was_updated = await this.dao.Put(this.config);

                if (!was_updated)
                    throw new ControllerManagerException("Could not update config");

            }

        }

        // @@@@@@@@@@@@@@@@@@@@@@@@@
        //    Main Funcionalities
        // @@@@@@@@@@@@@@@@@@@@@@@@@
        public SendingPacket Get() {
            return this.config == null ? SendErrors.ConfigNotExists() : new PacketSuccess(200,this.config.to_json());
        }

        public async Task<SendingPacket> Create(IDictionary<string,object> config_data) {

            if (this._ConfigExists())
                return new PacketFail(403,"Config already exists. Do not need creation");

            try {

                var config_dto = new ConfigDTO();

                config_dto.set_username((string) config_data["username"]);
                config_dto.set_password((string) config_data["password"]);

                if (config_data.ContainsKey("name")) config_dto.set_name((string?) config_data["name"]);
                if (config_data.ContainsKey("public")) config_dto.set_public((bool) config_data["public"]);
                if (config_data.ContainsKey("initialMoney")) config_dto.set_initial_money((double) config_data["initialMoney"]);
                if (config_data.ContainsKey("lostMoney")) config_dto.set_lost_money((double) config_data["lostMoney"]);
                if (config_data.ContainsKey("savedMoney")) config_dto.set_saved_money((double) config_data["savedMoney"]);

                var new_config = config_dto.extract();

                if (await this.dao.Put(new_config)) {
                    this.config = new_config;
                    return new PacketSuccess(201,new_config.to_json());
                }
                else
                    return new PacketFail(422,"Error while inserting config into database");

            }
            catch (ConfigDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        public async Task<SendingPacket> Update(IDictionary<string,object> config_data) {

            if (this._ConfigExists() == false)
                return SendErrors.ConfigNotExists();

            try {

                var config_dto = new ConfigDTO();

                config_dto.set_username((string) config_data["username"]);
                config_dto.set_password((string) config_data["password"]);

                if (config_data.ContainsKey("name")) config_dto.set_name((string?) config_data["name"]);
                if (config_data.ContainsKey("public")) config_dto.set_public((bool) config_data["public"]);
                if (config_data.ContainsKey("initialMoney")) config_dto.set_initial_money((double) config_data["initialMoney"]);
                if (config_data.ContainsKey("lostMoney")) config_dto.set_lost_money((double) config_data["lostMoney"]);
                if (config_data.ContainsKey("savedMoney")) config_dto.set_saved_money((double) config_data["savedMoney"]);

                var updated_config = config_dto.extract();

                if (await this.dao.Put(updated_config)) {
                    this.config = updated_config;
                    return new PacketSuccess(200,updated_config.to_json());
                }
                else
                    return new PacketFail(422,"Error while updating config of database");

            }
            catch (ConfigDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        public async Task<SendingPacket> Patch(IDictionary<string,object> config_data) {

            if (this._ConfigExists() == false)
                return SendErrors.ConfigNotExists();

            try {

                var config_dto = new ConfigDTO((await this.dao.Get())!);

                if (config_data.ContainsKey("username")) config_dto.set_username((string) config_data["username"]);
                if (config_data.ContainsKey("password")) config_dto.set_password((string) config_data["password"]);
                if (config_data.ContainsKey("name")) config_dto.set_name((string?) config_data["name"]);
                if (config_data.ContainsKey("public")) config_dto.set_public((bool) config_data["public"]);
                if (config_data.ContainsKey("initialMoney")) config_dto.set_initial_money((double) config_data["initialMoney"]);
                if (config_data.ContainsKey("lostMoney")) config_dto.set_lost_money((double) config_data["lostMoney"]);
                if (config_data.ContainsKey("savedMoney")) config_dto.set_saved_money((double) config_data["savedMoney"]);

                var updated_config = config_dto.extract();

                if (await this.dao.Put(updated_config)) {
                    this.config = updated_config;
                    return new PacketSuccess(200,updated_config.to_json());
                }
                else
                    return new PacketFail(422,"Error while updating config of database");


            }
            catch (ConfigDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        public async Task<SendingPacket> Delete() {

            if (this._ConfigExists() == false)
                return SendErrors.ConfigNotExists();

            if (await this.dao.Delete()) {
                Config? deleted_config = this.config;
                this.config = null;
                return new PacketSuccess(200,deleted_config!.to_json());
            }
            else
                return new PacketFail(422,"Error while deleting config from database");

        }


    }

}
