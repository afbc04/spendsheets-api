using Npgsql;
using Serilog;

namespace DAO {

    enum EntryTagsField {
        tag_id,
        name,
        description
    };

    public class EntryTagsDAO {

        
        private static Tag _serialize(NpgsqlDataReader r) {
            return new Tag(
                r.getLong((int) EntryTagsField.tag_id),
                r.getString((int) EntryTagsField.name),
                r.tryGetString((int) EntryTagsField.description)
            );
        }

        public async Task<bool> Contains(long entryID, long tagID) {

            const string sql = "SELECT COUNT(*) FROM VEntryTagsByTag WHERE entryId = @entryID AND tagId = @tagID;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@entryID",entryID);
                cmd.Parameters.AddWithValue("@tagID",tagID);
                var count = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                return count > 0;

            });

        }

        public async Task<bool> Delete(long entryID, long tagID) {

            const string sql = "DELETE FROM EntryTags WHERE entryId = @entryID AND tagId = @tagID";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@entryID",entryID);
                cmd.Parameters.AddWithValue("@tagID",tagID);
                var lines = await cmd.ExecuteNonQueryAsync();
                return lines > 0;

            });

        }

        public async Task<long> Clear(long entryID, IList<long>? tagIds) {

            string specific_ids = tagIds == null ? "" : "AND tagId = ANY(@ids)";
            string sql = $"DELETE FROM EntryTags WHERE entryId = @entryID {specific_ids};";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@entryID",entryID);

                if (tagIds != null && tagIds.Any())
                    cmd.Parameters.AddWithValue("@ids", tagIds!.ToArray());

                var deleted_rows_count = await cmd.ExecuteNonQueryAsync();
                return deleted_rows_count;

            });
            
        }

        public async Task<long> Batch(long entryID, IList<long> tagIds) {
           
            if (tagIds == null || tagIds.Count == 0)
                return 0;

            const string sql = @"
                INSERT INTO EntryTags (entryId, tagId)
                SELECT @entryID, t.id
                FROM UNNEST(@tagIds) AS input(tagId)
                JOIN Tags t ON t.id = input.tagId
                ON CONFLICT (entryId, tagId) DO NOTHING;
            ";

            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@entryID", entryID);
                cmd.Parameters.AddWithValue("@tagIds", tagIds.ToArray());

                var tags_inserted = await cmd.ExecuteNonQueryAsync();
                return tags_inserted;
            });

        }

        public async Task<bool> Put(long entryID, long tagID) {

            const string sql = @"
                INSERT INTO EntryTags (entryId, tagId)
                VALUES (@entryId, @tagId)
                ON CONFLICT (entryId, tagId) DO NOTHING;
            ";

            try {

                return await DAOUtils.Query(sql, async cmd => {

                    cmd.Parameters.AddWithValue("@entryID",entryID);
                    cmd.Parameters.AddWithValue("@tagID",tagID);

                    await cmd.ExecuteNonQueryAsync();
                    return true;

                });
            }
            catch (Exception ex) {
                Log.Error(ex, "Error adding tag to entry");
                return false;
            }

        }

        public async Task<DAOListing<Tag>> Values(Query query) {
            return await DAOUtils.List("VEntryTagsByTag","tagId, name, description",query, r => _serialize(r));
        }

    }
}
