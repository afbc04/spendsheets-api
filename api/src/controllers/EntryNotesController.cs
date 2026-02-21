using PacketHandlers;
using DAO;
using Nito.AsyncEx;
using Queries;
using DTO;

namespace Controller {

    public class EntryNotesController {

        public readonly AsyncReaderWriterLock Lock;
        private EntryNotesDAO dao;

        public EntryNotesController() {
            this.Lock = new();
            this.dao = new EntryNotesDAO();
        }

        public async Task<SendingPacket> Get(bool can_read, Entry entry, string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryNote? entry_note = await this.dao.Get(entry,id);
                return entry_note == null ? new PacketFail(404) : new PacketSuccess(200,EntryNoteResponse.ToJson(entry_note,can_read));

            });

        }

        public async Task<SendingPacket> List(bool can_read, Entry entry, QueriesRequest? query_request) {

            var query = QTO.EntryNotes(query_request,entry.ID);
            var values = await this.dao.Values(query);

            return new PacketSuccess(200,new Pageable(
                query.limit,
                query.page,
                values.count,
                values.list.Select(i => EntryNoteResponse.ToJson(i,can_read)).ToList()!)
                .to_json());

        }

        /*
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
                    return new PacketFail(417,"In order to delete specific movements, its required to provide a list containing valid tag IDs");
                
                if (ids.Count == 0)
                    return new PacketFail(417,"In order to delete specific movements, its required to provide a non-empty list of IDs");

                notes_deleted = await this.dao.Clear(entryID,ids);

            }
            else
                notes_deleted = await this.dao.Clear(entryID,null);

            return new PacketSuccess(200,new Dictionary<string,object> {
                ["movementsDeleted"] = notes_deleted,
                ["deleted"] = notes_deleted > 0
            });

        }*/

        public async Task<SendingPacket> Create(IDictionary<string,object> entry_note_data, Entry entry) {

            try {

                var entry_note_dto = new EntryNoteDTO();

                if (entry_note_data.ContainsKey("money")) entry_note_dto.set_money((double?) entry_note_data["money"]);
                if (entry_note_data.ContainsKey("note")) entry_note_dto.set_note((string?) entry_note_data["note"]);
                if (entry_note_data.ContainsKey("date")) entry_note_dto.set_date(DateOnly.Parse((string) entry_note_data["date"]));

                var new_entry_note = entry_note_dto.extract();
                entry.addMoneyDelta(entry_note_dto.getMoneyDelta());

                long? id = await this.dao.Create(entry,new_entry_note);

                if (id != null) {
                    new_entry_note.ID = (long) id;
                    return new PacketSuccess(201,EntryNoteResponse.ToJson(new_entry_note));
                }
                else
                    return new PacketFail(422,"Error while inserting entry note into database");

            }
            catch (EntryNoteDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        public async Task<SendingPacket> Delete(long entryID, string _id) {

            return new PacketFail(501);
            /*
            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryNote? entry_movement = await this.dao.Get(entryID,id);
                
                if (entry_movement == null)
                    return new PacketFail(404);
                else {
                    
                    if (await this.dao.Delete(entryID,id)) {
                        return new PacketSuccess(200,entry_movement.ToJson());
                    }
                    else
                        return new PacketFail(422,"Error while deleting entry movement from database");

                }

            });*/

        }

        public async Task<SendingPacket> Update(IDictionary<string,object> entry_movement_data, string _id, long entryID) {

            return new PacketFail(501);
            /*
            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryNote? entry_movement = await this.dao.Get(entryID,id);
                
                if (entry_movement == null)
                    return new PacketFail(404);
                else {

                    try {

                        var entry_note_dto = new EntryNoteDTO(id);

                        entry_note_dto.set_money((double) entry_movement_data["money"]);
                        if (entry_movement_data.ContainsKey("comment")) entry_note_dto.set_comment((string?) entry_movement_data["comment"]);
                        if (entry_movement_data.ContainsKey("date")) entry_note_dto.set_date(DateOnly.Parse((string) entry_movement_data["date"]));

                        var updated_entry_movement = entry_note_dto.extract();

                        if (await this.dao.Update(entryID,updated_entry_movement))
                            return new PacketSuccess(200,updated_entry_movement.ToJson());
                        else
                            return new PacketFail(422,"Error while updating entry movement of database");

                    }
                    catch (EntryNoteDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });*/

        }

        public async Task<SendingPacket> Patch(IDictionary<string,object> entry_movement_data, string _id, long entryID) {

            return new PacketFail(501);
            /*
            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryNote? entry_movement = await this.dao.Get(entryID,id);
                
                if (entry_movement == null)
                    return new PacketFail(404);
                else {

                    try {

                        var entry_note_dto = new EntryNoteDTO(entry_movement);

                        if (entry_movement_data.ContainsKey("money")) entry_note_dto.set_money((double) entry_movement_data["money"]);
                        if (entry_movement_data.ContainsKey("comment")) entry_note_dto.set_comment((string?) entry_movement_data["comment"]);
                        if (entry_movement_data.ContainsKey("date")) entry_note_dto.set_date(DateOnly.Parse((string) entry_movement_data["date"]));

                        var updated_entry_movement = entry_note_dto.extract();

                        if (await this.dao.Update(entryID,updated_entry_movement))
                            return new PacketSuccess(200,updated_entry_movement.ToJson());
                        else
                            return new PacketFail(422,"Error while updating entry movement of database");

                    }
                    catch (EntryNoteDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });*/

        }

    }

}