using Npgsql;
using NpgsqlTypes;
using Serilog;
using DTO;

namespace DAO {

    enum EntryNotesField {
        id,
        note,
        change_date
    };

    public class EntryNotesDAO {

        private static EntryNote _serialize(NpgsqlDataReader r) {
            return new EntryNote(
                r.getLong((int) EntryNotesField.id),
                r.getString((int) EntryNotesField.note),
                r.getDate((int) EntryNotesField.change_date)
            );
        }

        public async Task<EntryNote?> Get(long entryID, long ID) {

            const string sql = @"SELECT 
                    id, note, changeDate
                FROM EntryNotes WHERE id = @id AND entryId = @entryID;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id",ID);
                cmd.Parameters.AddWithValue("@entryID",entryID);
                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                    return _serialize(reader);

                return null;

            });

        }

        public async Task<bool> Contains(long entryID, long ID) {

            const string sql = "SELECT COUNT(*) FROM EntryNotes WHERE id = @id AND entryId = @entryID;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                cmd.Parameters.AddWithValue("@entryID",entryID);
                var count = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                return count > 0;

            });

        }

        public async Task<bool> Delete(long entryID, long ID) {

            const string sql = "DELETE FROM EntryNotes WHERE id = @id AND entryId = @entryID";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                cmd.Parameters.AddWithValue("@entryID",entryID);
                var lines = await cmd.ExecuteNonQueryAsync();
                return lines > 0;

            });

        }

        public async Task<long> Clear(long entryID, IList<long>? noteIds) {

            string specific_ids = noteIds == null ? "" : "AND id = ANY(@ids)";
            string sql = $"DELETE FROM EntryNotes WHERE entryId = @entryID {specific_ids};";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@entryID",entryID);

                if (noteIds != null && noteIds.Any())
                    cmd.Parameters.AddWithValue("@ids", noteIds!.ToArray());

                var deleted_rows_count = await cmd.ExecuteNonQueryAsync();
                return deleted_rows_count;

            });
            
        }

        public async Task<long?> Create(long entryID, EntryNote entry_note) {

            try {

                const string sql = @"
                    INSERT INTO EntryNotes 
                        (entryId, note, changeDate)
                    VALUES
                        (@entryId, @note, @changeDate)
                    RETURNING id";

                return await DAOUtils.Query<long?>(sql, async cmd => {

                    cmd.Parameters.Add("@entryId", NpgsqlDbType.Bigint)
                        .Value = entryID;

                    cmd.Parameters.Add("@note", NpgsqlDbType.Varchar)
                        .Value = entry_note.note;

                    cmd.Parameters.Add("@changeDate", NpgsqlDbType.Date)
                        .Value = entry_note.changeDate;

                    object? result = await cmd.ExecuteScalarAsync();
                    return result is long id ? id : null;

                });

            }
            catch (Exception ex) {
                Log.Error(ex.StackTrace ?? ex.Message);
                return null;
            }

        }

        public async Task<bool> Update(long entryID, EntryNote entry_note) {

            try {

                const string sql = @"
                    UPDATE EntryNotes
                    SET 
                        note = @note,
                        changeDate = @changeDate
                    WHERE id = @id AND entryId = @entryId";

                return await DAOUtils.Query(sql, async cmd => {

                    cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                        .Value = entry_note.ID;

                    cmd.Parameters.Add("@entryId", NpgsqlDbType.Bigint)
                        .Value = entryID;

                    cmd.Parameters.Add("@note", NpgsqlDbType.Varchar)
                        .Value = entry_note.note;
                    
                    cmd.Parameters.Add("@changeDate", NpgsqlDbType.Date)
                        .Value = entry_note.changeDate;

                    await cmd.ExecuteNonQueryAsync();
                    return true;

                });

            }
            catch (Exception ex) {
                Log.Error(ex.StackTrace ?? ex.Message);
                return false;
            }

        }

        public async Task<DAOListing<EntryNote>> Values(Query query) {
            return await DAOUtils.List("EntryNotes","id, note, changeDate",query, r => _serialize(r));
        }

        public async Task<IList<long>> Keys(long entryID) {

            string sql = "SELECT id FROM EntryNotes WHERE entryId = @entryId;";

            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.Add("@entryId", NpgsqlDbType.Bigint)
                    .Value = entryID;

                var list = new List<long>();
                await using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                    list.Add(reader.GetInt64(0));
                
                return list;

            });

        }

        public async Task<long> Size() {
            return await DAOUtils.Count("EntryNotes");
        }

        public async Task<bool> IsEmpty() {
            return await this.Size() == 0;
        }

    }
}
