using PacketHandlers;
using Controller;
using Queries;

public class EntryNotesManager {

    private TokenController token;
    private EntryController entry;
    private EntryNotesController entry_notes;

    public EntryNotesManager(EntryNotesController entry_notes, EntryController entry, TokenController token) {
        this.entry_notes = entry_notes;
        this.entry = entry;
        this.token = token;
    }

    public async Task<SendingPacket> List(string? extracted_token, string entry_id, QueriesRequest? query) {
        
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderAsync(token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entry_object) => {
                        using (await entry_notes.Lock.ReaderLockAsync())
                            return await entry_notes.List(AccessToken.CanRead(access_token),entry_object,query);
                    });
            });
        
    }

    /*
    public async Task<SendingPacket> Clear(string? extracted_token, string entry_id, QueriesRequest? query) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExistsAndHasMovements(entry,entry_id,async (entryID) => {
                        using (await entry_notes.Lock.WriterLockAsync())
                            return await entry_notes.Clear(query,entryID);
                    });
            });
        
    }*/

    public async Task<SendingPacket> Create(string extracted_token, IDictionary<string,object> request_data, string entry_id) {

        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriterAsync(token,extracted_token,async (access_token) => {
                using (await entry.Lock.WriterLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entry_object) => {
                        using (await entry_notes.Lock.WriterLockAsync())
                            return await entry_notes.Create(request_data,entry_object);
                    });
            });

    }
    
    public async Task<SendingPacket> Get(string extracted_token, string entry_id, string entry_notes_id) {

        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderAsync(token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entry_object) => {
                        using (await entry_notes.Lock.ReaderLockAsync())
                            return await entry_notes.Get(AccessToken.CanRead(access_token),entry_object,entry_notes_id);
                    });
            });

    }

    /*
    public async Task<SendingPacket> Delete(string extracted_token, string entry_id, string entry_notes_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExistsAndHasMovements(entry,entry_id,async (entryID) => {
                        using (await entry_notes.Lock.WriterLockAsync())
                            return await entry_notes.Delete(entryID,entry_notes_id);
                    });
            });

    }

    public async Task<SendingPacket> Patch(string extracted_token, IDictionary<string,object> request_data, string entry_id, string entry_notes_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExistsAndHasMovements(entry,entry_id,async (entryID) => {
                        using (await entry_notes.Lock.WriterLockAsync())
                            return await entry_notes.Patch(request_data,entry_notes_id,entryID);
                    });
            });

    }

    public async Task<SendingPacket> Put(string extracted_token, IDictionary<string,object> request_data, string entry_id, string entry_notes_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExistsAndHasMovements(entry,entry_id,async (entryID) => {
                        using (await entry_notes.Lock.WriterLockAsync())
                            return await entry_notes.Update(request_data,entry_notes_id,entryID);
                    });
            });

    }*/

}