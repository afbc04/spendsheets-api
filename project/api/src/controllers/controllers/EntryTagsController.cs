using PacketHandlers;
using DAO;
using Nito.AsyncEx;
using Queries;

namespace Controller {

    public class EntryTagsController {

        public readonly AsyncReaderWriterLock Lock;
        private EntryTagsDAO dao;

        public EntryTagsController() {
            this.Lock = new();
            this.dao = new EntryTagsDAO();
        }

        public async Task<SendingPacket> Get(long entryID, string _tagID) {

            return await ControllerHelper.IDIsNumber(_tagID, async (tagID) => {

                Tag? tag_of_entry = await this.dao.Get(entryID,tagID);
                return tag_of_entry == null ? new PacketFail(404) : new PacketSuccess(200,tag_of_entry.to_json());

            });

        }

        public async Task<SendingPacket> List(long entryID, QueriesRequest? query_request) {

            var query = QTO.EntryTags(query_request,entryID);
            var values = await this.dao.Values(query);

            return new PacketSuccess(200,new Pageable(
                query.limit,
                query.page,
                values.count,
                values.list.Select(i => i.to_json()).ToList()!)
                .to_json());

        }

        public async Task<SendingPacket> Clear(long entryID) {

            var tag_removed = await this.dao.Clear(entryID);

            return new PacketSuccess(200,new Dictionary<string,object> {
                ["tagRemoved"] = tag_removed,
                ["removed"] = tag_removed > 0
            });

        }

        public async Task<SendingPacket> Put(long entryID, Tag? tag) {

            if (tag == null)
                return new PacketFail(404,"Tag does not exists");

            var was_added = await this.dao.Put(entryID,tag.ID);
            return was_added == false ? new PacketFail(422,"Tag could not be added to the entry") : new PacketSuccess(201,"Tag added to the entry");

        }

        public async Task<SendingPacket> Delete(long entryID, Tag? tag) {

            if (tag == null)
                return new PacketFail(404,"Tag does not exists");

            var was_removed = await this.dao.Delete(entryID,tag.ID);
            return was_removed == false ? new PacketFail(422,"Tag could not be removed of the entry") : new PacketSuccess(200,"Tag removed from the entry");

        }

    }

}
