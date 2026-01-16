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

        public async Task<SendingPacket> Batch(IDictionary<string,object> entry_tags_data, long entryID) {

            List<long> list_of_tags = new();

            foreach (object id in (List<object>) entry_tags_data["tags"])
                list_of_tags.Add((long) id);

            var tags_inserted = await this.dao.Batch(entryID,list_of_tags);

            return new PacketSuccess(200,new Dictionary<string,object> {
                ["tagsAdded"] = tags_inserted,
                ["tagsRejected"] = list_of_tags.Count - tags_inserted,
                ["added"] = tags_inserted > 0
            });

        }

        public async Task<SendingPacket> Clear(QueriesRequest? query_request, long entryID) {

            bool clear_specific = query_request != null && query_request.queries.ContainsKey("ids");
            long tags_deleted;

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
                    return new PacketFail(417,"In order to delete specific tags, its required to provide a list containing valid tag IDs");
                
                if (ids.Count == 0)
                    return new PacketFail(417,"In order to delete specific tags, its required to provide a non-empty list of IDs");

                tags_deleted = await this.dao.Clear(entryID,ids);

            }
            else
                tags_deleted = await this.dao.Clear(entryID,null);

            return new PacketSuccess(200,new Dictionary<string,object> {
                ["tagsDeleted"] = tags_deleted,
                ["deleted"] = tags_deleted > 0
            });

        }

        public async Task<SendingPacket> Put(long entryID, Tag? tag) {

            if (tag == null)
                return new PacketFail(404,"Tag does not exists");

            var was_added = await this.dao.Put(entryID,tag.ID);
            return was_added == false ? 
                  new PacketFail(422,"Tag could not be added to the entry") 
                : new PacketSuccess(201,new Dictionary<string,object> {
                    ["info"] = "Tag was added to the entry"});

        }

        public async Task<SendingPacket> Delete(long entryID, Tag? tag) {

            if (tag == null)
                return new PacketFail(404,"Tag does not exists");

            var was_removed = await this.dao.Delete(entryID,tag.ID);
            return was_removed == false ? 
                  new PacketFail(422,"Tag could not be removed from the entry") 
                : new PacketSuccess(201,new Dictionary<string,object> {
                    ["info"] = "Tag was deleted from entry"});

        }

    }

}
