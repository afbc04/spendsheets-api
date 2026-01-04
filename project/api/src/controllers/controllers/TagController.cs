using PacketHandlers;
using DAO;
using Nito.AsyncEx;
using Queries;
using DTO;

namespace Controller {

    public class TagController {

        public readonly AsyncReaderWriterLock Lock;
        private TagDAO dao;

        public TagController() {
            this.Lock = new();
            this.dao = new TagDAO();
        }

        public async Task<Tag?> _Get(long ID) {
            return await this.dao.Get(ID);
        }

        public async Task<SendingPacket> Get(string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Tag? tag = await this.dao.Get(id);
                return tag == null ? new PacketFail(404) : new PacketSuccess(200,tag.to_json());

            });

        }

        public async Task<SendingPacket> List(QueriesRequest? query_request) {

            var query = QTO.Tag(query_request);
            var values = await this.dao.Values(query);

            return new PacketSuccess(200,new Pageable(
                query.limit,
                query.page,
                values.count,
                values.list.Select(i => i.to_json()).ToList()!)
                .to_json());

        }

        public async Task<SendingPacket> Clear() {

            long tags_deleted = await this.dao.Clear();
            return new PacketSuccess(200,new Dictionary<string,object> {
                ["tagsDeleted"] = tags_deleted,
                ["deleted"] = tags_deleted > 0
            });

        }

        public async Task<SendingPacket> Create(IDictionary<string,object> tag_data) {

            try {

                var tag_dto = new TagDTO();

                tag_dto.set_name((string) tag_data["name"]);
                if (tag_data.ContainsKey("description")) tag_dto.set_description((string?) tag_data["description"]);

                var new_tag = tag_dto.extract();
                long? id = await this.dao.Create(new_tag);

                if (id != null) {
                    new_tag.ID = (long) id;
                    return new PacketSuccess(201,new_tag.to_json());
                }
                else
                    return new PacketFail(422,"Error while inserting tag into database");

            }
            catch (TagDTOException ex) {
                return new PacketFail(417,ex.message);
            }

        }

        public async Task<SendingPacket> Delete(string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Tag? tag = await this.dao.Get(id);
                
                if (tag == null)
                    return new PacketFail(404);
                else {
                    
                    if (await this.dao.Delete(id)) {
                        return new PacketSuccess(200,tag.to_json());
                    }
                    else
                        return new PacketFail(422,"Error while deleting tag from database");

                }

            });

        }

        public async Task<SendingPacket> Update(IDictionary<string,object> tag_data, string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Tag? tag = await this.dao.Get(id);
                
                if (tag == null)
                    return new PacketFail(404);
                else {
                    
                    try {

                        var tag_dto = new TagDTO(id);

                        tag_dto.set_name((string) tag_data["name"]);
                        if (tag_data.ContainsKey("description")) tag_dto.set_description((string?) tag_data["description"]);

                        var updated_tag = tag_dto.extract();

                        if (await this.dao.Update(updated_tag))
                            return new PacketSuccess(200,updated_tag.to_json());
                        else
                            return new PacketFail(422,"Error while updating tag of database");

                    }
                    catch (TagDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });

        }

        public async Task<SendingPacket> Patch(IDictionary<string,object> tag_data, string _id) {

            return await ControllerHelper.IDIsNumber(_id, async (id) => {

                Tag? tag = await this.dao.Get(id);
                
                if (tag == null)
                    return new PacketFail(404);
                else {
                    
                    try {

                        var tag_dto = new TagDTO(tag);

                        if (tag_data.ContainsKey("name")) tag_dto.set_name((string) tag_data["name"]);
                        if (tag_data.ContainsKey("description")) tag_dto.set_description((string?) tag_data["description"]);

                        var updated_tag = tag_dto.extract();

                        if (await this.dao.Update(updated_tag))
                            return new PacketSuccess(200,updated_tag.to_json());
                        else
                            return new PacketFail(422,"Error while updating tag of database");

                    }
                    catch (TagDTOException ex) {
                        return new PacketFail(417,ex.message);
                    }

                }

            });

        }


    }

}
