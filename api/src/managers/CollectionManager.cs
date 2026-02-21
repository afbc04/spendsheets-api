using PacketHandlers;
using Controller;
using Queries;

public class CollectionManager {

    private TokenController token;
    private CategoryController category;
    private CollectionController collection;

    public CollectionManager(CollectionController collection,CategoryController category, TokenController token) {
        this.collection = collection;
        this.category = category;
        this.token = token;
    }

    public async Task<SendingPacket> List(string? extracted_token, QueriesRequest? query) {
        
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderAsync(token,extracted_token,async (access_token) => {
                using (await collection.Lock.ReaderLockAsync())
                    return await collection.List(AccessToken.CanRead(access_token),query);
            });
        
    }

    public async Task<SendingPacket> Clear(string? extracted_token, QueriesRequest? query) {
        
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriterAsync(token,extracted_token,async (access_token) => {
                using (await collection.Lock.WriterLockAsync())
                    return await collection.Clear(query);
            });
        
    }

    public async Task<SendingPacket> Map(string? extracted_token, IDictionary<string,object> request_data, QueriesRequest? query) {
        
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriterAsync(token,extracted_token,async (access_token) => {
                using (await collection.Lock.WriterLockAsync())
                    return await collection.Map(request_data,query);
            });
        
    }

    public async Task<SendingPacket> Create(string extracted_token, IDictionary<string,object> request_data) {

        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriterAsync(token,extracted_token,async (access_token) => {
                using (await category.Lock.ReaderLockAsync())
                using (await collection.Lock.WriterLockAsync()) {

                    Category? category = await _GetCategory(request_data);
                    return await collection.Create(request_data,category);

                }
            });

    }

    public async Task<SendingPacket> Get(string? extracted_token, string id) {
        
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderAsync(token,extracted_token,async (access_token) => {
                using (await collection.Lock.ReaderLockAsync())
                    return await collection.Get(AccessToken.CanRead(access_token),id);
            });
        
    }

    public async Task<SendingPacket> Delete(string extracted_token, string id) {

        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriterAsync(token,extracted_token,async (access_token) => {
                using (await collection.Lock.WriterLockAsync())
                    return await collection.Delete(id);
            });

    }

    public async Task<SendingPacket> Patch(IDictionary<string,object> request_data, string extracted_token, string id) {

        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriterAsync(token,extracted_token,async (access_token) => {
                using (await category.Lock.ReaderLockAsync())
                using (await collection.Lock.WriterLockAsync()) {

                    Category? category = await _GetCategory(request_data);
                    return await collection.Patch(request_data,id,category);

                }
            });

    }

    public async Task<SendingPacket> Update(IDictionary<string,object> request_data, string extracted_token, string id) {

        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriterAsync(token,extracted_token,async (access_token) => {
                using (await category.Lock.ReaderLockAsync())
                using (await collection.Lock.WriterLockAsync()) {

                    Category? category = await _GetCategory(request_data);
                    return await collection.Update(request_data,id,category);

                }
            });

    }


    private async Task<Category?> _GetCategory(IDictionary<string,object> request_data) {

        Category? category = null;
        if (request_data.ContainsKey("monthlyService") && request_data["monthlyService"] != null) {

            IDictionary<string,object> inner_monthly_service_data = (IDictionary<string,object>) request_data["monthlyService"];
            if (inner_monthly_service_data.ContainsKey("category"))
                category = await this.category._Get((long) inner_monthly_service_data["category"]);

        }

        return category;

    }

}