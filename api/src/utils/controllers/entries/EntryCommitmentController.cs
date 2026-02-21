using PacketHandlers;
using DAO;
using DTO;

/*
namespace Controller {

    public class EntryCommitmentController {

        private EntryDAO dao;

        public EntryCommitmentController(EntryDAO dao) {
            this.dao = dao;
        }

        public async Task<EntryCommitment?> _Get(long ID) {
            return await this.dao.GetCommitment(ID);
        }

        public async Task<SendingPacket> Create(IDictionary<string,object> entry_data, Category? category, MonthlyServiceSimple? monthly_service) {

            try {

                var entry_dto = new EntryCommitmentDTO();

                entry_dto.set_money_amount((double) entry_data["targetMoney"]);
                
                if (entry_data.ContainsKey("status")) entry_dto.set_status((string) entry_data["status"]);
                if (entry_data.ContainsKey("categoryId")) entry_dto.set_category(entry_data["categoryId"] != null, category);
                if (entry_data.ContainsKey("monthlyServiceId")) entry_dto.set_monthly_service(entry_data["monthlyServiceId"] != null, monthly_service);
                if (entry_data.ContainsKey("date")) entry_dto.set_date((DateOnly) entry_data["date"]);
                if (entry_data.ContainsKey("dueDate")) entry_dto.set_due_date((DateOnly?) entry_data["dueDate"]);
                if (entry_data.ContainsKey("description")) entry_dto.set_description((string?) entry_data["description"]);
                if (entry_data.ContainsKey("visible")) entry_dto.set_visible((bool) entry_data["visible"]);

                var new_entry = (EntryCommitment) entry_dto.extract();
                long? id = await this.dao.Create(new_entry);

                if (id != null) {
                    new_entry.ID = (long) id;
                    return new PacketSuccess(201,new EntryDetails(new_entry,category,monthly_service).ToJson());
                }
                else
                    return new PacketFail(422,"Error while inserting entry into database");

            }
            catch (EntryDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        public async Task<SendingPacket> Update(IDictionary<string,object> entry_data, long id, Category? category, MonthlyServiceSimple? monthly_service) {

            EntryCommitment? entry = await _Get(id);
                
            if (entry == null)
                return new PacketFail(404);
            else {
                    
                try {

                    var entry_dto = new EntryCommitmentDTO(id);

                    entry_dto.set_money_amount((double) entry_data["targetMoney"]);
                    
                    if (entry_data.ContainsKey("status")) entry_dto.set_status((string) entry_data["status"]);
                    if (entry_data.ContainsKey("categoryId")) entry_dto.set_category(entry_data["categoryId"] != null, category);
                    if (entry_data.ContainsKey("monthlyServiceId")) entry_dto.set_monthly_service(entry_data["monthlyServiceId"] != null, monthly_service);
                    if (entry_data.ContainsKey("date")) entry_dto.set_date((DateOnly) entry_data["date"]);
                    if (entry_data.ContainsKey("dueDate")) entry_dto.set_due_date((DateOnly?) entry_data["dueDate"]);
                    if (entry_data.ContainsKey("description")) entry_dto.set_description((string?) entry_data["description"]);
                    if (entry_data.ContainsKey("visible")) entry_dto.set_visible((bool) entry_data["visible"]);

                    var updated_entry = entry_dto.extract();

                    if (await this.dao.Update(updated_entry)) {
                        var entry_details = await this.dao.GetDetailed(id);
                        return entry_details != null ? 
                            new PacketSuccess(200,entry_details!.ToJson())
                            : new PacketFail(422,"Entry was updated, but couldn't get entry's information from database");
                    } else
                        return new PacketFail(422,"Error while updating entry of database");

                }
                catch (EntryDTOException ex) {
                    return new PacketFail(417,ex.message);
                }

            }

        }

        public async Task<SendingPacket> Patch(IDictionary<string,object> entry_data, long id, Category? category, MonthlyServiceSimple? monthly_service) {

            EntryCommitment? entry = await _Get(id);
                
            if (entry == null)
                return new PacketFail(404);
            else {
                    
                try {

                    var entry_dto = new EntryCommitmentDTO(entry);

                    if (entry_data.ContainsKey("draft")) entry_dto.set_draft((bool) entry_data["draft"]);
                    if (entry_data.ContainsKey("deleted")) entry_dto.set_deleted((bool) entry_data["deleted"]);
                    if (entry_data.ContainsKey("status")) entry_dto.set_status((string) entry_data["status"]);

                    if (entry_data.ContainsKey("targetMoney")) entry_dto.set_money_amount((double) entry_data["targetMoney"]);
                    if (entry_data.ContainsKey("categoryId")) entry_dto.set_category(entry_data["categoryId"] != null, category);
                    if (entry_data.ContainsKey("monthlyServiceId")) entry_dto.set_monthly_service(entry_data["monthlyServiceId"] != null, monthly_service);
                    if (entry_data.ContainsKey("date")) entry_dto.set_date((DateOnly) entry_data["date"]);
                    if (entry_data.ContainsKey("dueDate")) entry_dto.set_due_date((DateOnly?) entry_data["dueDate"]);
                    if (entry_data.ContainsKey("description")) entry_dto.set_description((string?) entry_data["description"]);
                    if (entry_data.ContainsKey("visible")) entry_dto.set_visible((bool) entry_data["visible"]);

                    var updated_entry = entry_dto.extract();

                    if (await this.dao.Update(updated_entry)) {
                        var entry_details = await this.dao.GetDetailed(id);
                        return entry_details != null ? 
                            new PacketSuccess(200,entry_details!.ToJson())
                            : new PacketFail(422,"Entry was updated, but couldn't get entry's information from database");
                    } else
                        return new PacketFail(422,"Error while updating entry of database");

                }
                catch (EntryDTOException ex) {
                    return new PacketFail(417,ex.message);
                }

            }

        }

    }

}
*/