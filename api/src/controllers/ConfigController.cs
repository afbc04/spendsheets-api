using PacketHandlers;
using DAO;
using Nito.AsyncEx;
using DTO;
using ConfigHandler;

namespace Controller {

    public class ConfigController {

        public readonly AsyncReaderWriterLock Lock;

        public ConfigController() {
            this.Lock = new();
        }

        public bool _IsSystemPublic() {

            Config config = Config.Get();
            return config.is_public;

        }

        // @@@@@@@@@@@@@@@@@@@@@@@@@
        //    Main Funcionalities
        // @@@@@@@@@@@@@@@@@@@@@@@@@
        public SendingPacket Get() {

            Config config = Config.Get();
            return new PacketSuccess(200,ConfigResponse.ToJson(config));

        }

        public SendingPacket Update(IDictionary<string,object> config_data) {

            try {

                var config_dto = new ConfigDTO();

                config_dto.set_name((string?) config_data["name"]);
                config_dto.set_public((bool) config_data["public"]);
                config_dto.set_initial_money((double) config_data["initialMoney"]);
                config_dto.set_lost_money((double) config_data["lostMoney"]);
                config_dto.set_saved_money((double) config_data["savedMoney"]);

                var updated_config = config_dto.extract();

                if (Config.Update(updated_config)) {
                    return Get();
                }
                else
                    return new PacketFail(422,"Error while updating config");

            }
            catch (ConfigDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        public SendingPacket Patch(IDictionary<string,object> config_data) {

            try {

                var config_dto = new ConfigDTO(Config.Get());

                if (config_data.ContainsKey("name")) config_dto.set_name((string?) config_data["name"]);
                if (config_data.ContainsKey("public")) config_dto.set_public((bool) config_data["public"]);
                if (config_data.ContainsKey("initialMoney")) config_dto.set_initial_money((double) config_data["initialMoney"]);
                if (config_data.ContainsKey("lostMoney")) config_dto.set_lost_money((double) config_data["lostMoney"]);
                if (config_data.ContainsKey("savedMoney")) config_dto.set_saved_money((double) config_data["savedMoney"]);

                var updated_config = config_dto.extract();

                if (Config.Update(updated_config)) {
                    return Get();
                }
                else
                    return new PacketFail(422,"Error while updating config");


            }
            catch (ConfigDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }


    }

}