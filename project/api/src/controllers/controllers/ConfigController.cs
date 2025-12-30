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

            this._does_config_exist = this._dao.get() != null;

        }

        public static SendingPacket send_error_config_does_not_exists() {
            return new PacketFail(404,"System is not configured. Create config at \"POST /v1.0/config\"");
        }

        public Config? get() {
            return this._dao.get();
        }

        public async Task<SendingPacket> Get() {

            Config? config = get();

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
                this._dao.put(new_config);
                this._does_config_exist = true;
                return new PacketSuccess(201,new_config.to_json());

            }
            catch (ConfigDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        public async Task<SendingPacket> Update(IDictionary<string,object> config_data) {

            if (!this._does_config_exist)
                return ConfigController.send_error_config_does_not_exists();

            try {

                var config_dto = new ConfigDTO(this._dao.get()!);

                // FIXME: Users might write 12.40€ instead of 1240€

                if (config_data.ContainsKey("username")) config_dto.set_username((string) config_data["username"]);
                if (config_data.ContainsKey("password")) config_dto.set_password((string) config_data["password"]);
                if (config_data.ContainsKey("name")) config_dto.set_name((string) config_data["name"]);
                if (config_data.ContainsKey("public")) config_dto.set_public((bool) config_data["public"]);
                if (config_data.ContainsKey("initialMoney")) config_dto.set_initial_money((long) config_data["initialMoney"]);
                if (config_data.ContainsKey("lostMoney")) config_dto.set_lost_money((long) config_data["lostMoney"]);
                if (config_data.ContainsKey("savedMoney")) config_dto.set_saved_money((long) config_data["savedMoney"]);

                var updated_config = config_dto.extract();
                this._dao.put(updated_config);
                return new PacketSuccess(200,updated_config.to_json());

            }
            catch (ConfigDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }


    }

}

/*

        /// #################################
        ///           PUBLIC METHODS
        /// #################################

        public async Task<SendingPacket> create(CountryCodeRequestWrapper request_wrapper) {

            var error_lists = validate_country_code(request_wrapper);
            if (error_lists.Count() > 0)
                return new PacketFail("Country code is not valid",417,new Dictionary<string,object>(){ ["errors"] = error_lists});

            CountryCode country_code_to_be_inserted = request_wrapper.convert();

            if (await this._model.contains(country_code_to_be_inserted.ID))
                return new PacketFail("ID is already registered",422);

            bool inserted_country_code = await this._model.insert(country_code_to_be_inserted);
            if (inserted_country_code == false)
                return new PacketFail("Failed when inserting country code into the database",422);

            return new PacketSuccess(201, country_code_to_be_inserted.to_json());

        }

        public async Task<SendingPacket> list(QueryCountryCode querie) {

            var model_list = await this._model.values(querie);
            var country_code_list = model_list.list.Select(cc => cc.to_json()).ToList();

            var page_output = new PageOutput(querie.get_page(),model_list.all_elements,country_code_list);
            
            return new PacketSuccess(200, page_output.to_json());

        }

        public async Task<bool> aux_contains(string ID) {
            return await this._model.contains(ID);
        }

        public async Task<long> aux_count() {
            return await this._model.size();
        }

        public async Task<SendingPacket> get(string id) {

            CountryCode? country_code = await this._model.get(id.ToUpper());
            if (country_code == null)
                return new PacketFail(404);

            return new PacketSuccess(200, country_code.to_json());

        }

        public async Task<SendingPacket> delete(string id) {

            id = id.ToUpper();

            CountryCode? country_code = await this._model.get(id);
            if (country_code == null)
                return new PacketFail(404);

            bool was_deleted = await this._model.delete(id);
            if (was_deleted == false)
                return new PacketFail("Failed when deleting country code in database",422);

            return new PacketSuccess(200, country_code.to_json());

        }

        public async Task<SendingPacket> update(string id, CountryCodeRequestWrapper request_wrapper) {

            id = id.ToUpper();

            CountryCode? country_code = await this._model.get(id);
            if (country_code == null)
                return new PacketFail(404);

            request_wrapper.auto_fill(country_code);

            var error_lists = validate_country_code(request_wrapper);
            if (error_lists.Count() > 0)
                return new PacketFail("Country code is not valid",417,new Dictionary<string,object>(){ ["errors"] = error_lists});

            var updated_country_code = request_wrapper.convert();

            bool was_country_code_updated = await this._model.update(updated_country_code);
            if (was_country_code_updated == false)
                return new PacketFail("Failed when updating country code into the database",422);

            return new PacketSuccess(200, updated_country_code.to_json());

        }

        /// #################################
        ///           PRIVATE METHODS
        /// #################################

        private IList<string> validate_country_code(CountryCodeRequestWrapper request) {

            var error_list = new List<string>();

            if (request.id.All(char.IsLetter) == false)
                error_list.Add("ID must only contains letters");
            if (request.id.Length != CountryCode.idLength)
                error_list.Add($"ID must be {CountryCode.idLength} letters");

            if (request.name is null)
                error_list.Add($"Name must exists");
            else if (request.name.Length > CountryCode.nameMaxLength)
                error_list.Add($"Name is too big. Cant be bigger than {CountryCode.nameMaxLength} characters");

            return error_list;

        }
    }
}
*/