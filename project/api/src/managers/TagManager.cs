using PacketHandlers;
using Controller;
using Queries;

public class TagManager {

    private ConfigController config;
    private TokenController token;
    private TagController tag;

    public TagManager(TagController tag, TokenController token, ConfigController config) {
        this.tag = tag;
        this.token = token;
        this.config = config;
    }

    public async Task<SendingPacket> List(string? extracted_token, QueriesRequest? query) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await tag.Lock.ReaderLockAsync())
                    return await tag.List(query);
            });
        
    }

    public async Task<SendingPacket> Create(string extracted_token, IDictionary<string,object> request_data) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await tag.Lock.WriterLockAsync())
                    return await tag.Create(request_data);
            });

    }

    public async Task<SendingPacket> Get(string? extracted_token, string id) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await tag.Lock.ReaderLockAsync())
                    return await tag.Get(id);
            });
        
    }

    public async Task<SendingPacket> Delete(string extracted_token, string id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await tag.Lock.WriterLockAsync())
                    return await tag.Delete(id);
            });

    }

    public async Task<SendingPacket> Update(IDictionary<string,object> request_data, string extracted_token, string id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await tag.Lock.WriterLockAsync())
                    return await tag.Update(request_data,id);
            });

    }

}