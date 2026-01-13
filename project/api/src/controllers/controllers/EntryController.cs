using PacketHandlers;
using DAO;
using Nito.AsyncEx;
using Queries;
using DTO;

namespace Controller {

    public class EntryController {

        public readonly AsyncReaderWriterLock Lock;
        private EntryDAO dao;

        private EntryTransactionController transactions_controller;

        public EntryController() {
            this.Lock = new();
            this.dao = new EntryDAO();
            this.transactions_controller = new EntryTransactionController(this.dao);
        }

        /*
        public async Task<Entry?> _Get(long ID) {
            return await this.dao.Get(ID);
        }*/

        public async Task<bool> _Contains(long ID) {
            return await this.dao.Contains(ID);
        }
        
        public async Task<SendingPacket> Get(string _id) {
            //return await this.dao.Get(_id);

            
            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryDetails? Entry = await this.dao.GetDetailed(id);
                return Entry == null ? new PacketFail(404) : new PacketSuccess(200,Entry.ToJson());

            });

        }

        /*
        public async Task<SendingPacket> List(QueriesRequest? query_request) {

            var query = QTO.Entry(query_request);
            var values = await this.dao.Values(query);

            return new PacketSuccess(200,new Pageable(
                query.limit,
                query.page,
                values.count,
                values.list.Select(i => i.to_json()).ToList()!)
                .to_json());

        }

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

        public async Task<SendingPacket> Create(IDictionary<string,object> entry_data, Category? category) {

            return await this.transactions_controller.Create(entry_data,category);

            /*
            try {

                var Entry_dto = new EntryDTO();

                Entry_dto.set_name((string) Entry_data["name"]);
                if (Entry_data.ContainsKey("description")) Entry_dto.set_description((string?) Entry_data["description"]);

                var new_Entry = Entry_dto.extract();
                long? id = await this.dao.Create(new_Entry);

                if (id != null) {
                    new_Entry.ID = (long) id;
                    return new PacketSuccess(201,new_Entry.to_json());
                }
                else
                    return new PacketFail(422,"Error while inserting Entry into database");

            }
            catch (EntryDTOException ex) {
                return new PacketFail(417,ex.message);
            }*/

        }

        /*
        public async Task<SendingPacket> Delete(string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Entry? Entry = await this.dao.Get(id);
                
                if (Entry == null)
                    return new PacketFail(404);
                else {
                    
                    if (await this.dao.Delete(id)) {
                        return new PacketSuccess(200,Entry.to_json());
                    }
                    else
                        return new PacketFail(422,"Error while deleting Entry from database");

                }

            });

        }

        public async Task<SendingPacket> Update(IDictionary<string,object> Entry_data, string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Entry? Entry = await this.dao.Get(id);
                
                if (Entry == null)
                    return new PacketFail(404);
                else {
                    
                    try {

                        var Entry_dto = new EntryDTO(id);

                        Entry_dto.set_name((string) Entry_data["name"]);
                        if (Entry_data.ContainsKey("description")) Entry_dto.set_description((string?) Entry_data["description"]);

                        var updated_Entry = Entry_dto.extract();

                        if (await this.dao.Update(updated_Entry))
                            return new PacketSuccess(200,updated_Entry.to_json());
                        else
                            return new PacketFail(422,"Error while updating Entry of database");

                    }
                    catch (EntryDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });

        }

        public async Task<SendingPacket> Patch(IDictionary<string,object> Entry_data, string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Entry? Entry = await this.dao.Get(id);
                
                if (Entry == null)
                    return new PacketFail(404);
                else {
                    
                    try {

                        var Entry_dto = new EntryDTO(Entry);

                        if (Entry_data.ContainsKey("name")) Entry_dto.set_name((string) Entry_data["name"]);
                        if (Entry_data.ContainsKey("description")) Entry_dto.set_description((string?) Entry_data["description"]);

                        var updated_Entry = Entry_dto.extract();

                        if (await this.dao.Update(updated_Entry))
                            return new PacketSuccess(200,updated_Entry.to_json());
                        else
                            return new PacketFail(422,"Error while updating Entry of database");

                    }
                    catch (EntryDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });

        }*/


    }

}
