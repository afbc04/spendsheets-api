using PacketHandlers;
using Controller;

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

    public async Task<SendingPacket> Create(IDictionary<string,object> request_data) {

        using (await config.Lock.WriterLockAsync()) {
            return await config.Create(request_data);
        }

    }

    public async Task<SendingPacket> Update(IDictionary<string,object> request_data, string? extracted_token) {

        using (await config.Lock.WriterLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) =>
                await config.Update(request_data)
            );

    }

}