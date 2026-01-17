using PacketHandlers;
using Controller;
using Queries;

public class EntryManager {

    private ConfigController config;
    private TokenController token;
    private CategoryController category;
    private MonthlyServiceController monthly_service;
    private EntryController entry;

    public EntryManager(EntryController entry, MonthlyServiceController monthly_service,CategoryController category, TokenController token, ConfigController config) {
        this.entry = entry;
        this.monthly_service = monthly_service;
        this.category = category;
        this.token = token;
        this.config = config;
    }

    public async Task<SendingPacket> List(string? extracted_token, QueriesRequest? query) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await entry.List(query);
            });
        
    }
    /*
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
        
    }*/

    public async Task<SendingPacket> Create(string extracted_token, IDictionary<string,object> request_data) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                
                using (await category.Lock.ReaderLockAsync())
                using (await monthly_service.Lock.ReaderLockAsync())
                using (await entry.Lock.WriterLockAsync()) {

                    Category? category = null;
                    if (request_data.ContainsKey("categoryId") && request_data["categoryId"] != null)
                        category = await this.category._Get((long) request_data["categoryId"]);

                    MonthlyServiceSimple? monthly_service = null;
                    if (request_data.ContainsKey("monthlyServiceId") && request_data["monthlyServiceId"] != null)
                        monthly_service = await this.monthly_service._Get((long) request_data["monthlyServiceId"]);

                    return await entry.Create(request_data,category,monthly_service);

                }
            });

    }

    public async Task<SendingPacket> Get(string? extracted_token, string id) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await entry.Get(id);
            });
        
    }
    
    public async Task<SendingPacket> Delete(string extracted_token, string id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.WriterLockAsync())
                    return await entry.Delete(id);
            });

    }

    public async Task<SendingPacket> Patch(IDictionary<string,object> request_data, string extracted_token, string id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                
                using (await category.Lock.ReaderLockAsync())
                using (await monthly_service.Lock.ReaderLockAsync())
                using (await entry.Lock.WriterLockAsync()) {

                    Category? category = null;
                    if (request_data.ContainsKey("categoryId") && request_data["categoryId"] != null)
                        category = await this.category._Get((long) request_data["categoryId"]);

                    MonthlyServiceSimple? monthly_service = null;
                    if (request_data.ContainsKey("monthlyServiceId") && request_data["monthlyServiceId"] != null)
                        monthly_service = await this.monthly_service._Get((long) request_data["monthlyServiceId"]);

                    return await entry.Patch(request_data,id,category,monthly_service);

                }
            });

    }

    public async Task<SendingPacket> Update(IDictionary<string,object> request_data, string extracted_token, string id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                
                using (await category.Lock.ReaderLockAsync())
                using (await monthly_service.Lock.ReaderLockAsync())
                using (await entry.Lock.WriterLockAsync()) {

                    Category? category = null;
                    if (request_data.ContainsKey("categoryId") && request_data["categoryId"] != null)
                        category = await this.category._Get((long) request_data["categoryId"]);

                    MonthlyServiceSimple? monthly_service = null;
                    if (request_data.ContainsKey("monthlyServiceId") && request_data["monthlyServiceId"] != null)
                        monthly_service = await this.monthly_service._Get((long) request_data["monthlyServiceId"]);

                    return await entry.Update(request_data,id,category,monthly_service);

                }
            });

    }

}