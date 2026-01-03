using PacketHandlers;
using Controller;

public class TokenManager {

    private ConfigController config;
    private TokenController token;

    public TokenManager(TokenController token, ConfigController config) {
        this.token = token;
        this.config = config;
    }

    public async Task<SendingPacket> Get() {

        using (await config.Lock.ReaderLockAsync())
            return await ManagerHelper.CheckConfig(config, async () => {
                using (await token.Lock.ReaderLockAsync())
                    return token.Get();
            });

    }

    public async Task<SendingPacket> Create(IDictionary<string,object> request_data) {

        using (await config.Lock.ReaderLockAsync())
            return await ManagerHelper.CheckConfig(config, async () => {
                using (await token.Lock.WriterLockAsync())
                    return token.Create(request_data,config.config!);
            });

    }

    public async Task<SendingPacket> Delete(string token_extracted) {

        using (await config.Lock.ReaderLockAsync())
            return await ManagerHelper.CheckConfig(config, async () => {
                using (await token.Lock.WriterLockAsync())
                    return token.Delete(token._GetToken(token_extracted));
            });

    }

    public async Task<SendingPacket> Update(IDictionary<string,object> request_data, string token_extracted) {

        using (await config.Lock.ReaderLockAsync())
            return await ManagerHelper.CheckConfig(config, async () => {
                using (await token.Lock.WriterLockAsync())
                    return token.Update(request_data,token._GetToken(token_extracted),config.config!);
            });

    }

}