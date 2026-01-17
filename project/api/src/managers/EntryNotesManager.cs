using PacketHandlers;
using Controller;
using Queries;

public class EntryNotesManager {

    private ConfigController config;
    private TokenController token;
    private EntryController entry;
    private EntryNotesController entry_note;

    public EntryNotesManager(EntryNotesController entry_note, EntryController entry, TokenController token, ConfigController config) {
        this.entry_note = entry_note;
        this.entry = entry;
        this.token = token;
        this.config = config;
    }

    public async Task<SendingPacket> List(string? extracted_token, string entry_id, QueriesRequest? query) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entryID) => {
                        using (await entry_note.Lock.ReaderLockAsync())
                            return await entry_note.List(entryID,query);
                    });
            });
        
    }

    public async Task<SendingPacket> Clear(string? extracted_token, string entry_id, QueriesRequest? query) {
        
        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entryID) => {
                        using (await entry_note.Lock.WriterLockAsync())
                            return await entry_note.Clear(query,entryID);
                    });
            });
        
    }

    public async Task<SendingPacket> Create(string extracted_token, IDictionary<string,object> request_data, string entry_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entryID) => {
                        using (await entry_note.Lock.WriterLockAsync())
                            return await entry_note.Create(request_data,entryID);
                    });
            });

    }
    
    public async Task<SendingPacket> Get(string extracted_token, string entry_id, string entry_note_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entryID) => {
                        using (await entry_note.Lock.ReaderLockAsync())
                            return await entry_note.Get(entryID,entry_note_id);
                    });
            });

    }

    public async Task<SendingPacket> Delete(string extracted_token, string entry_id, string entry_note_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenReaderPublic(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entryID) => {
                        using (await entry_note.Lock.WriterLockAsync())
                            return await entry_note.Delete(entryID,entry_note_id);
                    });
            });

    }

    public async Task<SendingPacket> Patch(string extracted_token, IDictionary<string,object> request_data, string entry_id, string entry_note_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entryID) => {
                        using (await entry_note.Lock.WriterLockAsync())
                            return await entry_note.Patch(request_data,entry_note_id,entryID);
                    });
            });

    }

    public async Task<SendingPacket> Update(string extracted_token, IDictionary<string,object> request_data, string entry_id, string entry_note_id) {

        using (await config.Lock.ReaderLockAsync())
        using (await token.Lock.ReaderLockAsync())
            return await ManagerHelper.WithTokenWriter(config,token,extracted_token,async (access_token) => {
                using (await entry.Lock.ReaderLockAsync())
                    return await ControllerHelper.CheckIfEntryExists(entry,entry_id,async (entryID) => {
                        using (await entry_note.Lock.WriterLockAsync())
                            return await entry_note.Update(request_data,entry_note_id,entryID);
                    });
            });

    }

}