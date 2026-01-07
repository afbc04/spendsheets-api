using PacketHandlers;
using DAO;
using Nito.AsyncEx;
using Queries;
using DTO;

namespace Controller {

    public class MonthlyServiceController {

        public readonly AsyncReaderWriterLock Lock;
        private MonthlyServiceDAO dao;

        public MonthlyServiceController() {
            this.Lock = new();
            this.dao = new MonthlyServiceDAO();
        }

        public async Task<MonthlyServiceSimple?> _Get(long ID) {
            return await this.dao.GetSimple(ID);
        }

        
        public async Task<SendingPacket> Get(string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                MonthlyServiceFull? monthlyService = await this.dao.GetFull(id);
                return monthlyService == null ? new PacketFail(404) : new PacketSuccess(200,monthlyService.to_json());

            });

        }

        public async Task<SendingPacket> List(QueriesRequest? query_request) {

            var query = QTO.MonthlyService(query_request);
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
            
            bool? active_filter = null;
            if (query_request != null && query_request.queries.ContainsKey("active"))
                active_filter = Convert.ToBoolean(query_request.queries["active"]!);
            
            long monthly_services_deleted;

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
                    return new PacketFail(417,"In order to delete specific monthly services, its required to provide a list containing valid tag IDs");
                
                if (ids.Count == 0)
                    return new PacketFail(417,"In order to delete specific monthly services, its required to provide a non-empty list of IDs");

                monthly_services_deleted = await this.dao.Clear(ids,active_filter);

            }
            else
                monthly_services_deleted = await this.dao.Clear(null,active_filter);

            return new PacketSuccess(200,new Dictionary<string,object> {
                ["monthlyServicesDeleted"] = monthly_services_deleted,
                ["deleted"] = monthly_services_deleted > 0
            });

        }

        public async Task<SendingPacket> Map(IDictionary<string,object> monthly_services_data, QueriesRequest? query_request) {

            bool clear_specific = query_request != null && query_request.queries.ContainsKey("ids");
            
            bool? active_filter = null;
            if (query_request != null && query_request.queries.ContainsKey("active"))
                active_filter = Convert.ToBoolean(query_request.queries["active"]!);
            
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
                    return new PacketFail(417,"In order to update specific monthly services, its required to provide a list containing valid tag IDs");
                
                if (ids.Count == 0)
                    return new PacketFail(417,"In order to update specific monthly services, its required to provide a non-empty list of IDs");

            }

            try {

                var monthly_service_dto = new MonthlyServiceMapDTO();

                if (monthly_services_data.ContainsKey("moneyAmount")) monthly_service_dto.set_money_amount((double?) monthly_services_data["moneyAmount"]);
                if (monthly_services_data.ContainsKey("active")) monthly_service_dto.set_active((bool) monthly_services_data["active"]);

                if (monthly_service_dto.does_updates_anything() == false)
                    return new PacketFail(417,"Data provided does not update anything");

                long monthly_services_updated = await this.dao.Map(monthly_service_dto,ids,active_filter);
                return new PacketSuccess(200,new Dictionary<string,object> {
                    ["monthlyServicesUpdated"] = monthly_services_updated,
                    ["updated"] = monthly_services_updated > 0
                });

            }
            catch (MonthlyServiceDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        public async Task<SendingPacket> Create(IDictionary<string,object> monthly_service_data, Category? category_related) {

            try {

                var monthly_service_dto = new MonthlyServiceDTO();

                monthly_service_dto.set_name((string) monthly_service_data["name"]);
                if (monthly_service_data.ContainsKey("description")) monthly_service_dto.set_description((string?) monthly_service_data["description"]);
                if (monthly_service_data.ContainsKey("categoryRelatedId")) monthly_service_dto.set_category_related(monthly_service_data["categoryRelatedId"] != null,category_related);
                if (monthly_service_data.ContainsKey("moneyAmount")) monthly_service_dto.set_money_amount((double?) monthly_service_data["moneyAmount"]);
                if (monthly_service_data.ContainsKey("active")) monthly_service_dto.set_active((bool) monthly_service_data["active"]);

                var new_monthly_service = monthly_service_dto.extract();
                long? id = await this.dao.Create(new_monthly_service);

                if (id != null) {
                    new_monthly_service.ID = (long) id;
                    var monthly_service_full = new MonthlyServiceFull(new_monthly_service,category_related);
                    return new PacketSuccess(201,monthly_service_full.to_json());
                }
                else
                    return new PacketFail(422,"Error while inserting monthly service into database");

            }
            catch (MonthlyServiceDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        
        public async Task<SendingPacket> Delete(string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                MonthlyServiceFull? monthly_service = await this.dao.GetFull(id);
                
                if (monthly_service == null)
                    return new PacketFail(404);
                else {
                    
                    if (await this.dao.Delete(id)) {
                        return new PacketSuccess(200,monthly_service.to_json());
                    }
                    else
                        return new PacketFail(422,"Error while deleting monthly service from database");

                }

            });

        }

        public async Task<SendingPacket> Update(IDictionary<string,object> monthly_service_data, string _id, Category? category_related) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                MonthlyServiceSimple? monthly_service = await this.dao.GetSimple(id);
                
                if (monthly_service == null)
                    return new PacketFail(404);
                else {
                    
                    try {

                        var monthly_service_dto = new MonthlyServiceDTO(monthly_service.ID);

                        monthly_service_dto.set_name((string) monthly_service_data["name"]);
                        if (monthly_service_data.ContainsKey("description")) monthly_service_dto.set_description((string?) monthly_service_data["description"]);
                        if (monthly_service_data.ContainsKey("categoryRelatedId")) monthly_service_dto.set_category_related(monthly_service_data["categoryRelatedId"] != null,category_related);
                        if (monthly_service_data.ContainsKey("moneyAmount")) monthly_service_dto.set_money_amount((double?) monthly_service_data["moneyAmount"]);
                        if (monthly_service_data.ContainsKey("active")) monthly_service_dto.set_active((bool) monthly_service_data["active"]);

                        var updated_monthly_service = monthly_service_dto.extract();

                        if (await this.dao.Update(updated_monthly_service)) {
                            var monthly_service_full = new MonthlyServiceFull(updated_monthly_service,category_related);
                            return new PacketSuccess(200,monthly_service_full.to_json());
                        }
                        else
                            return new PacketFail(422,"Error while updating monthly service of database");

                    }
                    catch (MonthlyServiceDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });

        }

        public async Task<SendingPacket> Patch(IDictionary<string,object> monthly_service_data, string _id, Category? category_related) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                MonthlyServiceSimple? monthly_service = await this.dao.GetSimple(id);
                
                if (monthly_service == null)
                    return new PacketFail(404);
                else {
                    
                    try {

                        var monthly_service_dto = new MonthlyServiceDTO(monthly_service);

                        if (monthly_service_data.ContainsKey("name")) monthly_service_dto.set_name((string) monthly_service_data["name"]);
                        if (monthly_service_data.ContainsKey("description")) monthly_service_dto.set_description((string?) monthly_service_data["description"]);
                        if (monthly_service_data.ContainsKey("categoryRelatedId")) monthly_service_dto.set_category_related(monthly_service_data["categoryRelatedId"] != null,category_related);
                        if (monthly_service_data.ContainsKey("moneyAmount")) monthly_service_dto.set_money_amount((double?) monthly_service_data["moneyAmount"]);
                        if (monthly_service_data.ContainsKey("active")) monthly_service_dto.set_active((bool) monthly_service_data["active"]);

                        var updated_monthly_service = monthly_service_dto.extract();

                        if (await this.dao.Update(updated_monthly_service)) {
                            var monthly_service_full = new MonthlyServiceFull(updated_monthly_service,category_related);
                            return new PacketSuccess(200,monthly_service_full.to_json());
                        }
                        else
                            return new PacketFail(422,"Error while updating monthly service of database");

                    }
                    catch (MonthlyServiceDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });

        }


    }

}
