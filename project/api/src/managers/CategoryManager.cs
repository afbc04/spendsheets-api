using PacketHandlers;
using Controller;
using Queries;

public class CategoryManager {

    private ConfigController config;
    private TokenController token;
    private CategoryController category;

    public CategoryManager(CategoryController category, TokenController token, ConfigController config) {
        this.category = category;
        this.token = token;
        this.config = config;
    }

    public async Task<SendingPacket> List(string? extracted_token, QueriesRequest? query) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await category.Lock.ReaderLockAsync())
                    return await category.List(query);
            });
        
    }

    public async Task<SendingPacket> Clear(string? extracted_token, QueriesRequest? query) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await category.Lock.WriterLockAsync())
                    return await category.Clear(query);
            });
        
    }

    public async Task<SendingPacket> Create(string extracted_token, IDictionary<string,object> request_data) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await category.Lock.WriterLockAsync())
                    return await category.Create(request_data);
            });

    }

    public async Task<SendingPacket> Get(string? extracted_token, string id) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await category.Lock.ReaderLockAsync())
                    return await category.Get(id);
            });
        
    }

    public async Task<SendingPacket> Delete(string extracted_token, string id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await category.Lock.WriterLockAsync())
                    return await category.Delete(id);
            });

    }

    public async Task<SendingPacket> Patch(IDictionary<string,object> request_data, string extracted_token, string id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await category.Lock.WriterLockAsync())
                    return await category.Patch(request_data,id);
            });

    }

    public async Task<SendingPacket> Update(IDictionary<string,object> request_data, string extracted_token, string id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await category.Lock.WriterLockAsync())
                    return await category.Update(request_data,id);
            });

    }

}