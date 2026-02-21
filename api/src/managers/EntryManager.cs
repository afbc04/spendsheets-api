using PacketHandlers;
using Controller;
using Queries;

public class EntryManager {

    private TokenController token;
    private CategoryController category;
    private CollectionController collection;
    private EntryController entry;

    public EntryManager(EntryController entry, CollectionController collection,CategoryController category, TokenController token) {
        this.entry = entry;
        this.collection = collection;
        this.category = category;
        this.token = token;
    }

    public async Task<SendingPacket> List(string? extracted_token, QueriesRequest? query) {
        
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderAsync(token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await entry.List(AccessToken.CanRead(access_token),query);
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

        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriterAsync(token,extracted_token,async (access_token) => {
                
                using (await category.Lock.ReaderLockAsync())
                using (await collection.Lock.ReaderLockAsync())
                using (await entry.Lock.WriterLockAsync()) {

                    Category? category = await _GetCategory(request_data);
                    Collection? collection = await _GetCollection(request_data);
                    return await entry.Create(request_data,category,collection);

                }
            });

    }

    public async Task<SendingPacket> Get(string? extracted_token, string id) {
        
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderAsync(token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await entry.Get(AccessToken.CanRead(access_token),id);
            });
        
    }
    
    public async Task<SendingPacket> Delete(string extracted_token, string id) {

        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriterAsync(token,extracted_token,async (access_token) => {
                using (await entry.Lock.WriterLockAsync())
                    return await entry.Delete(id);
            });

    }

    public async Task<SendingPacket> Patch(IDictionary<string,object> request_data, string extracted_token, string id) {

        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriterAsync(token,extracted_token,async (access_token) => {
                
                using (await category.Lock.ReaderLockAsync())
                using (await collection.Lock.ReaderLockAsync())
                using (await entry.Lock.WriterLockAsync()) {

                    Category? category = await _GetCategory(request_data);
                    Collection? collection = await _GetCollection(request_data);
                    return await entry.Patch(request_data,id,category,collection);

                }
            });

    }

    public async Task<SendingPacket> Update(IDictionary<string,object> request_data, string extracted_token, string id) {

        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriterAsync(token,extracted_token,async (access_token) => {
                
                using (await category.Lock.ReaderLockAsync())
                using (await collection.Lock.ReaderLockAsync())
                using (await entry.Lock.WriterLockAsync()) {

                    Category? category = await _GetCategory(request_data);
                    Collection? collection = await _GetCollection(request_data);
                    return await entry.Update(request_data,id,category,collection);

                }
            });

    }



    private async Task<Category?> _GetCategory(IDictionary<string,object> request_data) {

        Category? category = null;
        if (request_data.ContainsKey("categoryId") && request_data["categoryId"] != null)
            category = await this.category._Get((long) request_data["categoryId"]);

        return category;

    }

    private async Task<Collection?> _GetCollection(IDictionary<string,object> request_data) {

        Collection? collection = null;
        if (request_data.ContainsKey("collectionId") && request_data["collectionId"] != null)
            collection = await this.collection._Get((long) request_data["collectionId"]);

        return collection;

    }

}