using PacketHandlers;
using Controller;
using Queries;

public class EntryTagsManager {

    private ConfigController config;
    private TokenController token;
    private TagController tag;
    private EntryController entry;
    private EntryTagsController entry_tag;

    public EntryTagsManager(EntryTagsController entry_tag, EntryController entry, TagController tag, TokenController token, ConfigController config) {
        this.entry_tag = entry_tag;
        this.entry = entry;
        this.tag = tag;
        this.token = token;
        this.config = config;
    }

    public async Task<SendingPacket> List(string? extracted_token, string entry_id, QueriesRequest? query) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entryID) => {
                        using (await entry_tag.Lock.ReaderLockAsync())
                            return await entry_tag.List(entryID,query);
                    });
            });
        
    }

    public async Task<SendingPacket> Clear(string? extracted_token, string entry_id) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entryID) => {
                        using (await entry_tag.Lock.WriterLockAsync())
                            return await entry_tag.Clear(entryID);
                    });
            });
        
    }

    /*
    public async Task<SendingPacket> Create(string extracted_token, IDictionary<string,object> request_data) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await category.Lock.ReaderLockAsync())
                using (await monthly_service.Lock.ReaderLockAsync())
                using (await entry.Lock.WriterLockAsync()) {

                    Category? category = null;
                    if (request_data.ContainsKey("categoryId"))
                        category = await this.category._Get((long) request_data["categoryId"]);

                    return await entry.Create(request_data,category);

                }
            });

    }*/

    public async Task<SendingPacket> Get(string? extracted_token, string entry_id, string tag_id) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entryID) => {
                        using (await entry_tag.Lock.ReaderLockAsync())
                            return await entry_tag.Get(entryID,tag_id);
                    });
            });
        
    }
    
    public async Task<SendingPacket> Delete(string extracted_token, string entry_id, string tag_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await tag.Lock.ReaderLockAsync())
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entryID) => {
                        using (await entry_tag.Lock.WriterLockAsync()) {
                            var tag = await this.tag._Get(tag_id);
                            return await entry_tag.Delete(entryID,tag);
                        }
                    });
            });

    }

    public async Task<SendingPacket> Put(string extracted_token, string entry_id, string tag_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await tag.Lock.ReaderLockAsync())
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entryID) => {
                        using (await entry_tag.Lock.WriterLockAsync()) {
                            var tag = await this.tag._Get(tag_id);
                            return await entry_tag.Put(entryID,tag);
                        }
                    });
            });

    }

}