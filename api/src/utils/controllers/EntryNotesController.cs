using PacketHandlers;
using DAO;
using Nito.AsyncEx;
using Queries;
using DTO;

/*
namespace Controller {

    public class EntryNotesController {

        public readonly AsyncReaderWriterLock Lock;
        private EntryNotesDAO dao;

        public EntryNotesController() {
            this.Lock = new();
            this.dao = new EntryNotesDAO();
        }

        public async Task<SendingPacket> Get(long entryID, string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryNote? entry_note = await this.dao.Get(entryID,id);
                return entry_note == null ? new PacketFail(404) : new PacketSuccess(200,entry_note.ToJson());

            });

        }

        public async Task<SendingPacket> List(long entryID, QueriesRequest? query_request) {

            var query = QTO.EntryNotes(query_request,entryID);
            var values = await this.dao.Values(query);

            return new PacketSuccess(200,new Pageable(
                query.limit,
                query.page,
                values.count,
                values.list.Select(i => i.ToJson()).ToList()!)
                .to_json());

        }

        public async Task<SendingPacket> Clear(QueriesRequest? query_request, long entryID) {

            bool clear_specific = query_request != null && query_request.queries.ContainsKey("ids");
            long notes_deleted;

            if (clear_specific) {

                var ids = new List<long>();
                var ids_extracted = ((string) query_request!.queries["ids"]!).Split(',', StringSplitOptions.RemoveEmptyEntries);
                long? extracted_id;
                bool all_valid_ids = true;

                foreach (string id in ids_extracted) {

                    extracted_id = Utils.to_number(id);

                    if (extracted_id != null)
                        ids.Add((long) extracted_id);
                    else
                        all_valid_ids = false;

                }

                if (all_valid_ids == false)
                    return new PacketFail(417,"In order to delete specific notes, its required to provide a list containing valid tag IDs");
                
                if (ids.Count == 0)
                    return new PacketFail(417,"In order to delete specific notes, its required to provide a non-empty list of IDs");

                notes_deleted = await this.dao.Clear(entryID,ids);

            }
            else
                notes_deleted = await this.dao.Clear(entryID,null);

            return new PacketSuccess(200,new Dictionary<string,object> {
                ["notesDeleted"] = notes_deleted,
                ["deleted"] = notes_deleted > 0
            });

        }

        public async Task<SendingPacket> Create(IDictionary<string,object> entry_note_data, long entryID) {

            try {

                var entry_note_dto = new EntryNoteDTO();

                entry_note_dto.set_note((string) entry_note_data["note"]);

                var new_entry_note = entry_note_dto.extract();
                long? id = await this.dao.Create(entryID,new_entry_note);

                if (id != null) {
                    new_entry_note.ID = (long) id;
                    return new PacketSuccess(201,new_entry_note.ToJson());
                }
                else
                    return new PacketFail(422,"Error while inserting entry note into database");

            }
            catch (EntryNoteDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        public async Task<SendingPacket> Delete(long entryID, string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryNote? entry_note = await this.dao.Get(entryID,id);
                
                if (entry_note == null)
                    return new PacketFail(404);
                else {
                    
                    if (await this.dao.Delete(entryID,id)) {
                        return new PacketSuccess(200,entry_note.ToJson());
                    }
                    else
                        return new PacketFail(422,"Error while deleting entry note from database");

                }

            });

        }

        public async Task<SendingPacket> Update(IDictionary<string,object> entry_note_data, string _id, long entryID) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryNote? entry_note = await this.dao.Get(entryID,id);
                
                if (entry_note == null)
                    return new PacketFail(404);
                else {

                    try {

                        var entry_note_dto = new EntryNoteDTO(id);

                        entry_note_dto.set_note((string) entry_note_data["note"]);

                        var updated_entry_note = entry_note_dto.extract();

                        if (await this.dao.Update(entryID,updated_entry_note))
                            return new PacketSuccess(200,updated_entry_note.ToJson());
                        else
                            return new PacketFail(422,"Error while updating entry note of database");

                    }
                    catch (EntryNoteDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });

        }

        public async Task<SendingPacket> Patch(IDictionary<string,object> entry_note_data, string _id, long entryID) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryNote? entry_note = await this.dao.Get(entryID,id);
                
                if (entry_note == null)
                    return new PacketFail(404);
                else {

                    try {

                        var entry_note_dto = new EntryNoteDTO(entry_note);

                        if (entry_note_data.ContainsKey("note")) entry_note_dto.set_note((string) entry_note_data["note"]);

                        var updated_entry_note = entry_note_dto.extract();

                        if (await this.dao.Update(entryID,updated_entry_note))
                            return new PacketSuccess(200,updated_entry_note.ToJson());
                        else
                            return new PacketFail(422,"Error while updating entry note of database");

                    }
                    catch (EntryNoteDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });

        }

    }

}
*/