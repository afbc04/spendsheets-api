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
        private EntryCommitmentController commitments_controller;
        private EntrySavingsController savings_controller;

        public EntryController() {
            this.Lock = new();
            this.dao = new EntryDAO();
            this.transactions_controller = new EntryTransactionController(this.dao);
            this.commitments_controller = new EntryCommitmentController(this.dao);
            this.savings_controller = new EntrySavingsController(this.dao);
        }

        // FIXME: add other types
        public async Task<Entry?> _Get(long ID,EntryType type) {
            return type switch {
                EntryType.Transaction => await this.transactions_controller._Get(ID),
                EntryType.Commitment => await this.commitments_controller._Get(ID),
                EntryType.Saving => await this.savings_controller._Get(ID),
                _ => null
            };
        }

        public async Task<bool> _Contains(long ID) {
            return await this.dao.Contains(ID);
        }

        public async Task<EntryType?> _GetType(long ID) {
            return await this.dao.GetType(ID);
        }
        
        public async Task<SendingPacket> Get(string _id) {          
            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryDetails? Entry = await this.dao.GetDetailed(id);
                return Entry == null ? new PacketFail(404) : new PacketSuccess(200,Entry.ToJson());

            });
        }

        public async Task<SendingPacket> List(QueriesRequest? query_request) {

            var query = QTO.EntryList(query_request);
            var values = await this.dao.Values(query);

            return new PacketSuccess(200,new Pageable(
                query.limit,
                query.page,
                values.count,
                values.list.Select(i => i.ToJson()).ToList()!)
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

        public async Task<SendingPacket> Create(IDictionary<string,object> entry_data, Category? category, MonthlyServiceSimple? monthly_service) {

            try {
                
                var entry_extraction = new EntryExtractionDTO(entry_data,true,null);
                var final_entry_data = entry_extraction.getData();

                return entry_extraction.getType() switch {
                    EntryType.Transaction => await this.transactions_controller.Create(entry_data,category,monthly_service),
                    EntryType.Commitment => await this.commitments_controller.Create(entry_data,category,monthly_service),
                    EntryType.Saving => await this.savings_controller.Create(entry_data,category),
                    _ => new PacketFail(417,"Invalid type of entry")
                };


            }
            catch (EntryExtractionDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        public async Task<SendingPacket> Delete(string _id) {
            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryDetails? entry = await this.dao.GetDetailed(id);
                
                if (entry == null)
                    return new PacketFail(404);
                else {
                    
                    if (await this.dao.Delete(id)) {
                        return new PacketSuccess(200,entry.ToJson());
                    }
                    else
                        return new PacketFail(422,"Error while deleting entry from database");

                }

            });
        }

        public async Task<SendingPacket> Update(IDictionary<string,object> entry_data, string _id, Category? category, MonthlyServiceSimple? monthly_service) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryType? entry_type = await this.dao.GetType(id);
                if (entry_type == null)
                    return new PacketFail(404);
                else {

                    try {
                        
                        var entry_extraction = new EntryExtractionDTO(entry_data,true,entry_type);
                        var final_entry_data = entry_extraction.getData();

                        return entry_extraction.getType() switch {
                            EntryType.Transaction => await this.transactions_controller.Update(entry_data,id,category,monthly_service),
                            EntryType.Commitment => await this.commitments_controller.Update(entry_data,id,category,monthly_service),
                            EntryType.Saving => await this.savings_controller.Update(entry_data,id,category),
                            _ => new PacketFail(417,"Invalid type of entry")
                        };


                    }
                    catch (EntryExtractionDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }
            });

        }

        public async Task<SendingPacket> Patch(IDictionary<string,object> entry_data, string _id, Category? category, MonthlyServiceSimple? monthly_service) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                EntryType? entry_type = await this.dao.GetType(id);
                if (entry_type == null)
                    return new PacketFail(404);
                else {

                    try {
                        
                        var entry_extraction = new EntryExtractionDTO(entry_data,false,entry_type);
                        var final_entry_data = entry_extraction.getData();

                        return entry_extraction.getType() switch {
                            EntryType.Transaction => await this.transactions_controller.Patch(entry_data,id,category,monthly_service),
                            EntryType.Commitment => await this.commitments_controller.Patch(entry_data,id,category,monthly_service),
                            EntryType.Saving => await this.savings_controller.Patch(entry_data,id,category),
                            _ => new PacketFail(417,"Invalid type of entry")
                        };


                    }
                    catch (EntryExtractionDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });

        }

    }

}
