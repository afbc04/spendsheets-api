using PacketHandlers;
using DAO;
using Nito.AsyncEx;
using Queries;
using DTO;

namespace Controller {

    public class EntryController {

        public readonly AsyncReaderWriterLock Lock;
        private EntryDAO dao;

        public EntryController() {
            this.Lock = new();
            this.dao = new EntryDAO();
        }

        public async Task<Entry?> _Get(long ID) {
            return await this.dao.Get(ID);
        }

        public async Task<bool> _Contains(long ID) {
            return await this.dao.Contains(ID);
        }
    
        public async Task<SendingPacket> Get(bool can_read, string _id) {          
            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryDetails? entry = await this.dao.GetDetailed(id);
                return entry == null ? new PacketFail(404) : new PacketSuccess(200,EntryResponse.ToJson(entry,can_read));

            });
        }

        public async Task<SendingPacket> List(bool can_read, QueriesRequest? query_request) {

            var query = QTO.EntryList(query_request);
            var values = await this.dao.Values(query);

            return new PacketSuccess(200,new Pageable(
                query.limit,
                query.page,
                values.count,
                values.list.Select(i => EntryListResponse.ToJson(i,can_read)).ToList()!)
                .to_json());

        }

        /*
        public async Task<SendingPacket> Clear(QueriesRequest? query_request) {

            bool clear_specific = query_request != null && query_request.queries.ContainsKey("ids");
            long categories_deleted;

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
                    return new PacketFail(417,"In order to delete specific categories, its required to provide a list containing valid tag IDs");
                
                if (ids.Count == 0)
                    return new PacketFail(417,"In order to delete specific categories, its required to provide a non-empty list of IDs");

                categories_deleted = await this.dao.Clear(ids);

            }
            else
                categories_deleted = await this.dao.Clear(null);

            return new PacketSuccess(200,new Dictionary<string,object> {
                ["categoriesDeleted"] = categories_deleted,
                ["deleted"] = categories_deleted > 0
            });

        }*/

        public async Task<SendingPacket> Create(IDictionary<string,object> entry_data, Category? category, Collection? collection) {

            try {

                var entry_dto = new EntryDTO();

                entry_dto.set_type((string) entry_data["type"]);
                entry_dto.set_money_target((double?) entry_data["targetMoney"]);
                entry_dto.set_money_amount((double) entry_data["actualMoney"]);
                
                if (entry_data.ContainsKey("date")) entry_dto.set_date(DateOnly.Parse((string) entry_data["date"]));
                if (entry_data.ContainsKey("dueDate")) entry_dto.set_due_date(entry_data["dueDate"] != null ? DateOnly.Parse((string) entry_data["dueDate"]) : null);
                
                if (entry_data.ContainsKey("categoryId")) entry_dto.set_category(entry_data["categoryId"] != null, category);
                if (entry_data.ContainsKey("collectionId")) entry_dto.set_collection(entry_data["collectionId"] != null, collection);
                if (entry_data.ContainsKey("description")) entry_dto.set_description((string?) entry_data["description"]);
                if (entry_data.ContainsKey("visible")) entry_dto.set_visible((bool) entry_data["visible"]);
                if (entry_data.ContainsKey("public")) entry_dto.set_public((bool) entry_data["public"]);
                if (entry_data.ContainsKey("active")) entry_dto.set_active((bool) entry_data["active"]);
                
                if (entry_data.ContainsKey("status")) entry_dto.set_status((string) entry_data["status"]);

                var new_entry = entry_dto.extract();
                long? id = await this.dao.Create(new_entry);

                if (id != null) {
                    new_entry.ID = (long) id;
                    return new PacketSuccess(201,EntryResponse.ToJson(new_entry,category,collection));
                }
                else
                    return new PacketFail(422,"Error while inserting entry into database");

            }
            catch (EntryDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        public async Task<SendingPacket> Delete(string _id) {
            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryDetails? entry = await this.dao.GetDetailed(id);
                
                if (entry == null)
                    return new PacketFail(404);
                else {
                    
                    return await this.dao.Delete(id)
                        ? new PacketSuccess(200,EntryResponse.ToJson(entry))
                        : new PacketFail(422,"Error while deleting entry from database");

                }

            });
        }

        public async Task<SendingPacket> Update(IDictionary<string,object> entry_data, string _id, Category? category, Collection? collection) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Entry? entry = await this.dao.Get(id);
                if (entry == null)
                    return new PacketFail(404);
                else {

                    try {

                        var entry_dto = new EntryDTO(id);

                        entry_dto.set_type((string) entry_data["type"]);
                        entry_dto.set_money_target((double?) entry_data["targetMoney"]);
                        entry_dto.set_money_amount((double) entry_data["actualMoney"]);
                        
                        if (entry_data.ContainsKey("date")) entry_dto.set_date(DateOnly.Parse((string) entry_data["date"]));
                        if (entry_data.ContainsKey("dueDate")) entry_dto.set_due_date(entry_data["dueDate"] != null ? DateOnly.Parse((string) entry_data["dueDate"]) : null);
                        
                        if (entry_data.ContainsKey("categoryId")) entry_dto.set_category(entry_data["categoryId"] != null, category);
                        if (entry_data.ContainsKey("collectionId")) entry_dto.set_collection(entry_data["collectionId"] != null, collection);
                        if (entry_data.ContainsKey("description")) entry_dto.set_description((string?) entry_data["description"]);
                        if (entry_data.ContainsKey("visible")) entry_dto.set_visible((bool) entry_data["visible"]);
                        if (entry_data.ContainsKey("public")) entry_dto.set_public((bool) entry_data["public"]);
                        if (entry_data.ContainsKey("active")) entry_dto.set_active((bool) entry_data["active"]);
                        
                        if (entry_data.ContainsKey("status")) entry_dto.set_status((string) entry_data["status"]);

                        var updated_entry = entry_dto.extract();

                        return await this.dao.Update(updated_entry)
                            ? new PacketSuccess(200,EntryResponse.ToJson(updated_entry,category,collection))
                            : new PacketFail(422,"Error while updating entry of database");

                    }
                    catch (EntryDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }
            });

        }

        public async Task<SendingPacket> Patch(IDictionary<string,object> entry_data, string _id, Category? category, Collection? collection) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Entry? entry = await this.dao.Get(id);
                if (entry == null)
                    return new PacketFail(404);
                else {

                    try {

                        var entry_dto = new EntryDTO(entry);

                        if (entry_data.ContainsKey("type")) entry_dto.set_type((string) entry_data["type"]);
                        if (entry_data.ContainsKey("targetMoney")) entry_dto.set_money_target((double?) entry_data["targetMoney"]);
                        if (entry_data.ContainsKey("actualMoney")) entry_dto.set_money_amount((double) entry_data["actualMoney"]);
                        
                        if (entry_data.ContainsKey("date")) entry_dto.set_date(DateOnly.Parse((string) entry_data["date"]));
                        if (entry_data.ContainsKey("dueDate")) entry_dto.set_due_date(entry_data["dueDate"] != null ? DateOnly.Parse((string) entry_data["dueDate"]) : null);
                        
                        if (entry_data.ContainsKey("categoryId")) entry_dto.set_category(entry_data["categoryId"] != null, category);
                        if (entry_data.ContainsKey("collectionId")) entry_dto.set_collection(entry_data["collectionId"] != null, collection);
                        if (entry_data.ContainsKey("description")) entry_dto.set_description((string?) entry_data["description"]);
                        if (entry_data.ContainsKey("visible")) entry_dto.set_visible((bool) entry_data["visible"]);
                        if (entry_data.ContainsKey("public")) entry_dto.set_public((bool) entry_data["public"]);
                        if (entry_data.ContainsKey("active")) entry_dto.set_active((bool) entry_data["active"]);
                        
                        if (entry_data.ContainsKey("draft")) entry_dto.set_draft((bool) entry_data["draft"]);
                        if (entry_data.ContainsKey("deleted")) entry_dto.set_deleted((bool) entry_data["deleted"]);
                        if (entry_data.ContainsKey("status")) entry_dto.set_status((string) entry_data["status"]);

                        var updated_entry = entry_dto.extract();

                        return await this.dao.Update(updated_entry)
                            ? new PacketSuccess(200,EntryResponse.ToJson(updated_entry,category,collection))
                            : new PacketFail(422,"Error while updating entry of database");

                    }
                    catch (EntryDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }
            });   

        }

    }

}
