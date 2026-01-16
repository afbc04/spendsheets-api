using PacketHandlers;
using DAO;
using Nito.AsyncEx;
using Queries;
using DTO;

namespace Controller {

    public class EntryTransactionController {

        private EntryDAO dao;

        public EntryTransactionController(EntryDAO dao) {
            this.dao = dao;
        }

        public async Task<EntryTransaction?> _Get(long ID) {
            return await this.dao.GetTransaction(ID);
        }

        /*
        public async Task<SendingPacket> List(QueriesRequest? query_request) {

            var query = QTO.Category(query_request);
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

        public async Task<SendingPacket> Create(IDictionary<string,object> entry_data, Category? category, MonthlyServiceSimple? monthly_service) {

            try {

                var entry_dto = new EntryTransactionDTO();

                entry_dto.set_money_amount((double) entry_data["actualMoney"]);

                if (entry_data.ContainsKey("categoryId")) entry_dto.set_category(entry_data["categoryId"] != null, category);
                if (entry_data.ContainsKey("monthlyServiceId")) entry_dto.set_monthly_service(entry_data["monthlyServiceId"] != null, monthly_service);
                if (entry_data.ContainsKey("date")) entry_dto.set_date((DateOnly) entry_data["date"]);
                if (entry_data.ContainsKey("description")) entry_dto.set_description((string?) entry_data["description"]);
                if (entry_data.ContainsKey("visible")) entry_dto.set_visible((bool) entry_data["visible"]);
                if (entry_data.ContainsKey("status")) entry_dto.set_status((string) entry_data["status"]);

                var new_entry = entry_dto.extract();
                long? id = await this.dao.Create(new_entry);

                if (id != null) {
                    new_entry.ID = (long) id;
                    return new PacketSuccess(201,new EntryDetails(new_entry,category,monthly_service).ToJson());
                }
                else
                    return new PacketFail(422,"Error while inserting entry into database");

            }
            catch (EntryTransactionDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        /*
        public async Task<SendingPacket> Delete(string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Category? category = await this.dao.Get(id);
                
                if (category == null)
                    return new PacketFail(404);
                else {
                    
                    if (await this.dao.Delete(id)) {
                        return new PacketSuccess(200,category.to_json());
                    }
                    else
                        return new PacketFail(422,"Error while deleting category from database");

                }

            });

        }*/

        public async Task<SendingPacket> Update(IDictionary<string,object> entry_data, long id, Category? category, MonthlyServiceSimple? monthly_service) {

            EntryTransaction? entry = await _Get(id);
                
            if (entry == null)
                return new PacketFail(404);
            else {
                    
                try {

                    var entry_dto = new EntryTransactionDTO(id);

                    entry_dto.set_money_amount((double) entry_data["actualMoney"]);

                    if (entry_data.ContainsKey("categoryId")) entry_dto.set_category(entry_data["categoryId"] != null, category);
                    if (entry_data.ContainsKey("monthlyServiceId")) entry_dto.set_monthly_service(entry_data["monthlyServiceId"] != null, monthly_service);
                    if (entry_data.ContainsKey("date")) entry_dto.set_date((DateOnly) entry_data["date"]);
                    if (entry_data.ContainsKey("description")) entry_dto.set_description((string?) entry_data["description"]);
                    if (entry_data.ContainsKey("visible")) entry_dto.set_visible((bool) entry_data["visible"]);
                    if (entry_data.ContainsKey("status")) entry_dto.set_status((string) entry_data["status"]);

                    var updated_entry = entry_dto.extract();

                    if (await this.dao.Update(updated_entry)) {
                        var entry_details = await this.dao.GetDetailed(id);
                        return entry_details != null ? 
                            new PacketSuccess(200,entry_details!.ToJson())
                            : new PacketFail(422,"Entry was updated, but couldn't get entry's information from database");
                    } else
                        return new PacketFail(422,"Error while updating entry of database");

                }
                catch (EntryTransactionDTOException ex) {
                    return new PacketFail(417,ex.message);
                }

            }

        }

        public async Task<SendingPacket> Patch(IDictionary<string,object> entry_data, long id, Category? category, MonthlyServiceSimple? monthly_service) {

            EntryTransaction? entry = await _Get(id);
                
            if (entry == null)
                return new PacketFail(404);
            else {
                    
                try {

                    var entry_dto = new EntryTransactionDTO(entry);

                    if (entry_data.ContainsKey("actualMoney")) entry_dto.set_money_amount((double) entry_data["actualMoney"]);
                    if (entry_data.ContainsKey("categoryId")) entry_dto.set_category(entry_data["categoryId"] != null, category);
                    if (entry_data.ContainsKey("monthlyServiceId")) entry_dto.set_monthly_service(entry_data["monthlyServiceId"] != null, monthly_service);
                    if (entry_data.ContainsKey("date")) entry_dto.set_date((DateOnly) entry_data["date"]);
                    if (entry_data.ContainsKey("description")) entry_dto.set_description((string?) entry_data["description"]);
                    if (entry_data.ContainsKey("visible")) entry_dto.set_visible((bool) entry_data["visible"]);
                    if (entry_data.ContainsKey("status")) entry_dto.set_status((string) entry_data["status"]);

                    var updated_entry = entry_dto.extract();

                    if (await this.dao.Update(updated_entry)) {
                        var entry_details = await this.dao.GetDetailed(id);
                        return entry_details != null ? 
                            new PacketSuccess(200,entry_details!.ToJson())
                            : new PacketFail(422,"Entry was updated, but couldn't get entry's information from database");
                    } else
                        return new PacketFail(422,"Error while updating entry of database");

                }
                catch (EntryTransactionDTOException ex) {
                    return new PacketFail(417,ex.message);
                }

            }

        }

    }

}
