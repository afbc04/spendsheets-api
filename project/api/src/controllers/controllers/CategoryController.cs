using PacketHandlers;
using DAO;
using Nito.AsyncEx;
using Queries;
using DTO;

namespace Controller {

    public class CategoryController {

        public readonly AsyncReaderWriterLock Lock;
        private CategoryDAO dao;

        public CategoryController() {
            this.Lock = new();
            this.dao = new CategoryDAO();
        }

        public async Task<Category?> _Get(long ID) {
            return await this.dao.Get(ID);
        }

        public async Task<SendingPacket> Get(string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Category? category = await this.dao.Get(id);
                return category == null ? new PacketFail(404) : new PacketSuccess(200,category.to_json());

            });

        }

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

        }

        public async Task<SendingPacket> Create(IDictionary<string,object> category_data) {

            try {

                var category_dto = new CategoryDTO();

                category_dto.set_name((string) category_data["name"]);
                if (category_data.ContainsKey("description")) category_dto.set_description((string?) category_data["description"]);

                var new_category = category_dto.extract();
                long? id = await this.dao.Create(new_category);

                if (id != null) {
                    new_category.ID = (long) id;
                    return new PacketSuccess(201,new_category.to_json());
                }
                else
                    return new PacketFail(422,"Error while inserting category into database");

            }
            catch (CategoryDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

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

        }

        public async Task<SendingPacket> Update(IDictionary<string,object> category_data, string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Category? category = await this.dao.Get(id);
                
                if (category == null)
                    return new PacketFail(404);
                else {
                    
                    try {

                        var category_dto = new CategoryDTO(id);

                        category_dto.set_name((string) category_data["name"]);
                        if (category_data.ContainsKey("description")) category_dto.set_description((string?) category_data["description"]);

                        var updated_category = category_dto.extract();

                        if (await this.dao.Update(updated_category))
                            return new PacketSuccess(200,updated_category.to_json());
                        else
                            return new PacketFail(422,"Error while updating category of database");

                    }
                    catch (CategoryDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });

        }

        public async Task<SendingPacket> Patch(IDictionary<string,object> category_data, string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Category? category = await this.dao.Get(id);
                
                if (category == null)
                    return new PacketFail(404);
                else {
                    
                    try {

                        var category_dto = new CategoryDTO(category);

                        if (category_data.ContainsKey("name")) category_dto.set_name((string) category_data["name"]);
                        if (category_data.ContainsKey("description")) category_dto.set_description((string?) category_data["description"]);

                        var updated_category = category_dto.extract();

                        if (await this.dao.Update(updated_category))
                            return new PacketSuccess(200,updated_category.to_json());
                        else
                            return new PacketFail(422,"Error while updating category of database");

                    }
                    catch (CategoryDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });

        }


    }

}
