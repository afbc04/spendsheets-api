using PacketHandlers;
using Controller;

namespace Manager {

    public class ConfigManager {

        private ConfigController config;
        private TokenController token;

        public ConfigManager(ConfigController config, TokenController token) {
            this.config = config;
            this.token = token;
        }

        public async Task<SendingPacket> Get() {

            using (await config.Lock.ReaderLockAsync()) {
                return config.Get();
            }

        }

        public async Task<SendingPacket> Patch(IDictionary<string,object> request_data, string? extracted_token) {

            using (await config.Lock.WriterLockAsync())
            using (await token.Lock.WriterLockAsync())
                return ManagerHelper.WithTokenWriter(token,extracted_token, (access_token) => {
                    var response = config.Patch(request_data);
                    token._SetSystemVisibility(config._IsSystemPublic());
                    return response;
                });

        }

        public async Task<SendingPacket> Update(IDictionary<string,object> request_data, string? extracted_token) {

            using (await config.Lock.WriterLockAsync())
            using (await token.Lock.WriterLockAsync())
                return ManagerHelper.WithTokenWriter(token,extracted_token, (access_token) => {
                    var response = config.Update(request_data);
                    token._SetSystemVisibility(config._IsSystemPublic());
                    return response;
                });

        }

    }

}