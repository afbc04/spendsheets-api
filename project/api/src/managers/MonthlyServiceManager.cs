using PacketHandlers;
using Controller;
using Queries;

public class MonthlyServiceManager {

    private ConfigController config;
    private TokenController token;
    private CategoryController category;
    private MonthlyServiceController monthly_service;

    public MonthlyServiceManager(MonthlyServiceController monthly_service,CategoryController category, TokenController token, ConfigController config) {
        this.monthly_service = monthly_service;
        this.category = category;
        this.token = token;
        this.config = config;
    }

    public async Task<SendingPacket> List(string? extracted_token, QueriesRequest? query) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await monthly_service.Lock.ReaderLockAsync())
                    return await monthly_service.List(query);
            });
        
    }

    public async Task<SendingPacket> Clear(string? extracted_token, QueriesRequest? query) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await monthly_service.Lock.WriterLockAsync())
                    return await monthly_service.Clear(query);
            });
        
    }

    public async Task<SendingPacket> Map(string? extracted_token, IDictionary<string,object> request_data, QueriesRequest? query) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await monthly_service.Lock.WriterLockAsync())
                    return await monthly_service.Map(request_data,query);
            });
        
    }

    public async Task<SendingPacket> Create(string extracted_token, IDictionary<string,object> request_data) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await category.Lock.ReaderLockAsync())
                using (await monthly_service.Lock.WriterLockAsync()) {

                    Category? category = null;
                    if (request_data.ContainsKey("categoryRelatedId") && request_data["categoryRelatedId"] != null)
                        category = await this.category._Get((long) request_data["categoryRelatedId"]);

                    return await monthly_service.Create(request_data,category);

                }
            });

    }

    public async Task<SendingPacket> Get(string? extracted_token, string id) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await monthly_service.Lock.ReaderLockAsync())
                    return await monthly_service.Get(id);
            });
        
    }

    public async Task<SendingPacket> Delete(string extracted_token, string id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await monthly_service.Lock.WriterLockAsync())
                    return await monthly_service.Delete(id);
            });

    }

    public async Task<SendingPacket> Patch(IDictionary<string,object> request_data, string extracted_token, string id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await category.Lock.ReaderLockAsync())
                using (await monthly_service.Lock.WriterLockAsync()) {

                    Category? category = null;
                    if (request_data.ContainsKey("categoryRelatedId") && request_data["categoryRelatedId"] != null)
                        category = await this.category._Get((long) request_data["categoryRelatedId"]);

                    return await monthly_service.Patch(request_data,id,category);

                }
            });

    }

    public async Task<SendingPacket> Update(IDictionary<string,object> request_data, string extracted_token, string id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await category.Lock.ReaderLockAsync())
                using (await monthly_service.Lock.WriterLockAsync()) {

                    Category? category = null;
                    if (request_data.ContainsKey("categoryRelatedId") && request_data["categoryRelatedId"] != null)
                        category = await this.category._Get((long) request_data["categoryRelatedId"]);

                    return await monthly_service.Update(request_data,id,category);

                }
            });

    }

}