using PacketHandlers;
using DAO;
using Nito.AsyncEx;
using Queries;
using DTO;

namespace Controller {

    public class CollectionController {

        public readonly AsyncReaderWriterLock Lock;
        private CollectionDAO dao;

        public CollectionController() {
            this.Lock = new();
            this.dao = new CollectionDAO();
        }

        public async Task<Collection?> _Get(long ID) {
            return await this.dao.Get(ID);
        }

        
        public async Task<SendingPacket> Get(bool can_read, string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                CollectionDetails? collection = await this.dao.GetDetailed(id);
                return collection == null ? new PacketFail(404) : new PacketSuccess(200,CollectionResponse.ToJson(collection,can_read));

            });

        }

        public async Task<SendingPacket> List(bool can_read, QueriesRequest? query_request) {

            var query = QTO.Collection(can_read,query_request);
            var values = await this.dao.Values(query);

            return new PacketSuccess(200,new Pageable(
                query.limit,
                query.page,
                values.count,
                values.list.Select(i => CollectionListResponse.ToJson(i,can_read)).ToList()!)
                .to_json());

        }

        public async Task<SendingPacket> Clear(QueriesRequest? query_request) {

            bool clear_specific = query_request != null && query_request.queries.ContainsKey("ids");
            
            bool? active_filter = null;
            if (query_request != null && query_request.queries.ContainsKey("active"))
                active_filter = Convert.ToBoolean(query_request.queries["active"]!);

            bool? is_monthly_service = null;
            if (query_request != null && query_request.queries.ContainsKey("monthlyService"))
                is_monthly_service = Convert.ToBoolean(query_request.queries["monthlyService"]!);
            
            long collections_deleted;

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
                    return new PacketFail(417,"In order to delete specific collections, its required to provide a list containing valid tag IDs");
                
                if (ids.Count == 0)
                    return new PacketFail(417,"In order to delete specific collections, its required to provide a non-empty list of IDs");

                collections_deleted = await this.dao.Clear(ids,is_monthly_service,active_filter);

            }
            else
                collections_deleted = await this.dao.Clear(null,is_monthly_service,active_filter);

            return new PacketSuccess(200,new Dictionary<string,object> {
                ["collectionsDeleted"] = collections_deleted,
                ["deleted"] = collections_deleted > 0
            });

        }

        public async Task<SendingPacket> Map(IDictionary<string,object> collections_data, QueriesRequest? query_request) {

            bool clear_specific = query_request != null && query_request.queries.ContainsKey("ids");
            
            bool? active_filter = null;
            if (query_request != null && query_request.queries.ContainsKey("active"))
                active_filter = Convert.ToBoolean(query_request.queries["active"]!);

            bool? is_monthly_service = null;
            if (query_request != null && query_request.queries.ContainsKey("monthlyService"))
                is_monthly_service = Convert.ToBoolean(query_request.queries["monthlyService"]!);
            
            List<long>? ids = null;

            if (clear_specific) {

                ids = new List<long>();
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
                    return new PacketFail(417,"In order to update specific collections, its required to provide a list containing valid tag IDs");
                
                if (ids.Count == 0)
                    return new PacketFail(417,"In order to update specific collections, its required to provide a non-empty list of IDs");

            }

            try {

                var collection_dto = new CollectionMapDTO();

                if (collections_data.ContainsKey("monthlyServiceActive")) collection_dto.set_active((bool) collections_data["monthlyServiceActive"]);

                if (collection_dto.does_updates_anything() == false)
                    return new PacketFail(417,"Data provided does not update anything");

                long collections_updated = await this.dao.Map(collection_dto,ids,active_filter);
                return new PacketSuccess(200,new Dictionary<string,object> {
                    ["collectionsUpdated"] = collections_updated,
                    ["updated"] = collections_updated > 0
                });

            }
            catch (CollectionDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        public async Task<SendingPacket> Create(IDictionary<string,object> collection_data, Category? category_related) {

            try {

                var collection_dto = new CollectionDTO();

                collection_dto.set_name((string) collection_data["name"]);
                if (collection_data.ContainsKey("description")) collection_dto.set_description((string?) collection_data["description"]);
                
                bool is_monthly_service = collection_data.ContainsKey("monthlyService") && collection_data["monthlyService"] != null;
                if (is_monthly_service) {

                    IDictionary<string,object> monthly_service_data = (IDictionary<string,object>) collection_data["monthlyService"];
                    
                    if (monthly_service_data.ContainsKey("category")) collection_dto.set_category(monthly_service_data["category"] != null,category_related);
                    if (monthly_service_data.ContainsKey("moneyAmount")) collection_dto.set_money_amount((double?) monthly_service_data["moneyAmount"]);
                    if (monthly_service_data.ContainsKey("active")) collection_dto.set_monthly_service_active((bool) monthly_service_data["active"]);

                }

                var new_collection = collection_dto.extract(is_monthly_service);
                long? id = await this.dao.Create(new_collection);

                if (id != null) {
                    new_collection.ID = (long) id;
                    return new PacketSuccess(201,CollectionResponse.ToJson(new_collection,category_related));
                }
                else
                    return new PacketFail(422,"Error while inserting collection into database");

            }
            catch (CollectionDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        
        public async Task<SendingPacket> Delete(string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                CollectionDetails? collection = await this.dao.GetDetailed(id);
                
                if (collection == null)
                    return new PacketFail(404);
                else {
                    
                    return await this.dao.Delete(id)
                        ? new PacketSuccess(200,CollectionResponse.ToJson(collection))
                        : new PacketFail(422,"Error while deleting collection from database");

                }

            });

        }

        public async Task<SendingPacket> Update(IDictionary<string,object> collection_data, string _id, Category? category_related) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Collection? collection = await this.dao.Get(id);
                
                if (collection == null)
                    return new PacketFail(404);
                else {
                    
                    try {

                        var collection_dto = new CollectionDTO(collection.ID);

                        collection_dto.set_name((string) collection_data["name"]);
                        if (collection_data.ContainsKey("description")) collection_dto.set_description((string?) collection_data["description"]);
                        
                        bool is_monthly_service = collection_data.ContainsKey("monthlyService") && collection_data["monthlyService"] != null;
                        if (is_monthly_service) {

                            IDictionary<string,object> monthly_service_data = (IDictionary<string,object>) collection_data["monthlyService"];
                            
                            if (monthly_service_data.ContainsKey("category")) collection_dto.set_category(monthly_service_data["category"] != null,category_related);
                            if (monthly_service_data.ContainsKey("moneyAmount")) collection_dto.set_money_amount((double?) monthly_service_data["moneyAmount"]);
                            if (monthly_service_data.ContainsKey("active")) collection_dto.set_monthly_service_active((bool) monthly_service_data["active"]);

                        }

                        var updated_collection = collection_dto.extract(is_monthly_service);

                        return await this.dao.Update(updated_collection)
                            ? new PacketSuccess(200,CollectionResponse.ToJson(updated_collection,category_related))
                            : new PacketFail(422,"Error while updating collection of database");

                    }
                    catch (CollectionDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });

        }

        public async Task<SendingPacket> Patch(IDictionary<string,object> collection_data, string _id, Category? category_related) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                CollectionDetails? collection = await this.dao.GetDetailed(id);
                bool new_category_provided = false;
                
                if (collection == null)
                    return new PacketFail(404);
                else {
                    
                    try {

                        var collection_dto = new CollectionDTO(collection.collection);

                        if (collection_data.ContainsKey("name")) collection_dto.set_name((string) collection_data["name"]);
                        if (collection_data.ContainsKey("description")) collection_dto.set_description((string?) collection_data["description"]);
                        
                        bool is_monthly_service = collection_data.ContainsKey("monthlyService") && collection_data["monthlyService"] != null;
                        if (is_monthly_service) {

                            IDictionary<string,object> monthly_service_data = (IDictionary<string,object>) collection_data["monthlyService"];
                            new_category_provided = monthly_service_data.ContainsKey("category");
                            
                            if (monthly_service_data.ContainsKey("category")) collection_dto.set_category(monthly_service_data["category"] != null,category_related);
                            if (monthly_service_data.ContainsKey("moneyAmount")) collection_dto.set_money_amount((double?) monthly_service_data["moneyAmount"]);
                            if (monthly_service_data.ContainsKey("active")) collection_dto.set_monthly_service_active((bool) monthly_service_data["active"]);

                        }

                        var updated_collection = collection_dto.extract(is_monthly_service);

                        return await this.dao.Update(updated_collection)
                            ? new PacketSuccess(200,CollectionResponse.ToJson(updated_collection,new_category_provided ? category_related : collection.category))
                            : new PacketFail(422,"Error while updating collection of database");

                    }
                    catch (CollectionDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });

        }


    }

}
