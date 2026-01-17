using PacketHandlers;
using Controller;
using Queries;

public class EntryMovementsManager {

    private ConfigController config;
    private TokenController token;
    private EntryController entry;
    private EntryMovementsController entry_movement;

    public EntryMovementsManager(EntryMovementsController entry_movement, EntryController entry, TokenController token, ConfigController config) {
        this.entry_movement = entry_movement;
        this.entry = entry;
        this.token = token;
        this.config = config;
    }

    public async Task<SendingPacket> List(string? extracted_token, string entry_id, QueriesRequest? query) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExistsAndHasMovements(entry,entry_id,async (entryID) => {
                        using (await entry_movement.Lock.ReaderLockAsync())
                            return await entry_movement.List(entryID,query);
                    });
            });
        
    }

    public async Task<SendingPacket> Clear(string? extracted_token, string entry_id, QueriesRequest? query) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExistsAndHasMovements(entry,entry_id,async (entryID) => {
                        using (await entry_movement.Lock.WriterLockAsync())
                            return await entry_movement.Clear(query,entryID);
                    });
            });
        
    }

    public async Task<SendingPacket> Create(string extracted_token, IDictionary<string,object> request_data, string entry_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExistsAndHasMovements(entry,entry_id,async (entryID) => {
                        using (await entry_movement.Lock.WriterLockAsync())
                            return await entry_movement.Create(request_data,entryID);
                    });
            });

    }
    
    public async Task<SendingPacket> Get(string extracted_token, string entry_id, string entry_movement_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExistsAndHasMovements(entry,entry_id,async (entryID) => {
                        using (await entry_movement.Lock.ReaderLockAsync())
                            return await entry_movement.Get(entryID,entry_movement_id);
                    });
            });

    }

    public async Task<SendingPacket> Delete(string extracted_token, string entry_id, string entry_movement_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExistsAndHasMovements(entry,entry_id,async (entryID) => {
                        using (await entry_movement.Lock.WriterLockAsync())
                            return await entry_movement.Delete(entryID,entry_movement_id);
                    });
            });

    }

    public async Task<SendingPacket> Patch(string extracted_token, IDictionary<string,object> request_data, string entry_id, string entry_movement_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExistsAndHasMovements(entry,entry_id,async (entryID) => {
                        using (await entry_movement.Lock.WriterLockAsync())
                            return await entry_movement.Patch(request_data,entry_movement_id,entryID);
                    });
            });

    }

    public async Task<SendingPacket> Put(string extracted_token, IDictionary<string,object> request_data, string entry_id, string entry_movement_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExistsAndHasMovements(entry,entry_id,async (entryID) => {
                        using (await entry_movement.Lock.WriterLockAsync())
                            return await entry_movement.Update(request_data,entry_movement_id,entryID);
                    });
            });

    }

}