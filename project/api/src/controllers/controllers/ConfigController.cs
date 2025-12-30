using PacketHandlers;
using Models;
using Nito.AsyncEx;
using Queries;
using Pages;
using DTO;

namespace Controller {

    public class ConfigController {

        public readonly AsyncReaderWriterLock Lock;
        private ConfigDAO _dao;

        private bool _does_config_exist;

        public ConfigController() {
            this.Lock = new();
            this._dao = new ConfigDAO();
        }

        public static SendingPacket send_error_config_does_not_exists() {
            return new PacketFail(404,"System is not configured. Create config at \"POST /v1.0/config\"");
        }

        public async Task load_settings() {
            this._does_config_exist = await this._dao.contains();
        }

        public async Task<Config?> get() {
            return await this._dao.get();
        }

        public async Task<SendingPacket> Get() {

            Config? config = await get();

            if (config == null)
                return ConfigController.send_error_config_does_not_exists();
            else
                return new PacketSuccess(200,config.to_json());

        }

        public async Task<SendingPacket> Create(IDictionary<string,object> config_data) {

            if (this._does_config_exist)
                return new PacketFail(403,"Config already exists. Do not need creation");

            try {

                var config_dto = new ConfigDTO();

                config_dto.set_username((string) config_data["username"]);
                config_dto.set_password((string) config_data["password"]);

                // FIXME: Users might write 12.40€ instead of 1240€

                if (config_data.ContainsKey("name")) config_dto.set_name((string) config_data["name"]);
                if (config_data.ContainsKey("public")) config_dto.set_public((bool) config_data["public"]);
                if (config_data.ContainsKey("initialMoney")) config_dto.set_initial_money((long) config_data["initialMoney"]);
                if (config_data.ContainsKey("lostMoney")) config_dto.set_lost_money((long) config_data["lostMoney"]);
                if (config_data.ContainsKey("savedMoney")) config_dto.set_saved_money((long) config_data["savedMoney"]);

                var new_config = config_dto.extract();

                if (await this._dao.put(new_config)) {
                    this._does_config_exist = true;
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

            if (!this._does_config_exist)
                return ConfigController.send_error_config_does_not_exists();

            try {

                var config_dto = new ConfigDTO((await this._dao.get())!);

                // FIXME: Users might write 12.40€ instead of 1240€

                if (config_data.ContainsKey("username")) config_dto.set_username((string) config_data["username"]);
                if (config_data.ContainsKey("password")) config_dto.set_password((string) config_data["password"]);
                if (config_data.ContainsKey("name")) config_dto.set_name((string) config_data["name"]);
                if (config_data.ContainsKey("public")) config_dto.set_public((bool) config_data["public"]);
                if (config_data.ContainsKey("initialMoney")) config_dto.set_initial_money((long) config_data["initialMoney"]);
                if (config_data.ContainsKey("lostMoney")) config_dto.set_lost_money((long) config_data["lostMoney"]);
                if (config_data.ContainsKey("savedMoney")) config_dto.set_saved_money((long) config_data["savedMoney"]);

                var updated_config = config_dto.extract();

                if (await this._dao.put(updated_config)) {
                    this._does_config_exist = true;
                    return new PacketSuccess(200,updated_config.to_json());
                }
                else
                    return new PacketFail(422,"Error while updating config of database");


            }
            catch (ConfigDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }


    }

}
