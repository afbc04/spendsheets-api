using Npgsql;
using NpgsqlTypes;
using Serilog;
using DTO;

namespace DAO {

    enum EntryNotesField {
        id,
        money,
        note,
        date
    };

    public class EntryNotesDAO {

        private static EntryNote _serialize(NpgsqlDataReader r) =>
            new(
                r.getLong((int) EntryNotesField.id),
                r.getDate((int) EntryNotesField.date),
                r.tryGetInt((int) EntryNotesField.money),
                r.tryGetString((int) EntryNotesField.note)
            );

        public async Task<EntryNote?> Get(Entry entry, long ID) {

            const string sql = @"
                SELECT 
                    id, money, note, date
                FROM EntryNotes 
                WHERE 
                    id = @id AND entryId = @entryID;";

            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                    .Value = ID;

                cmd.Parameters.Add("@entryID", NpgsqlDbType.Bigint)
                    .Value = entry.ID;

                await using var reader = await cmd.ExecuteReaderAsync();
                return await reader.ReadAsync() ? _serialize(reader) : null;

            });

        }

        public async Task<bool> Contains(long entryID, long ID) {

            const string sql = "SELECT EXISTS (SELECT 1 FROM EntryNotes WHERE id = @id AND entryId = @entryID);";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                    .Value = ID;

                cmd.Parameters.Add("@entryID", NpgsqlDbType.Bigint)
                    .Value = entryID;

                return (bool) (await cmd.ExecuteScalarAsync() ?? false);

            });

        }

        public async Task<bool> Delete(Entry entry, long ID) {

            await using var conn = new NpgsqlConnection(DAOManager.connection_string);
            await conn.OpenAsync();

            await using var transaction = await conn.BeginTransactionAsync();

            try {

                // 1. Updating note
                await using (var cmd = conn.CreateCommand()) {

                    cmd.Transaction = transaction;

                    cmd.CommandText = "DELETE FROM EntryNotes WHERE id = @id AND entryId = @entryID";

                    cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                        .Value = ID;

                    cmd.Parameters.Add("@entryID", NpgsqlDbType.Bigint)
                        .Value = entry.ID;

                    var lines = await cmd.ExecuteNonQueryAsync();
                    if (lines == 0)
                        throw new Exception("Error while deleting entry note");

                }

                // 2. Updating Entry
                await using (var cmd = conn.CreateCommand()) {

                    cmd.Transaction = transaction;
                    if (await _updateEntry(cmd,entry) == false)
                        throw new Exception("Error while updating entry");
                }

                // 3. Commit
                await transaction.CommitAsync();
                return true;

            }
            catch (Exception ex) {
                await transaction.RollbackAsync();
                Log.Error(ex, ex.Message);
                return false;
            }

        }

        /*
        public async Task<long> Clear(long entryID, IList<long>? NoteIds) {

            string specific_ids = NoteIds == null ? "" : "AND id = ANY(@ids)";
            string sql = $"DELETE FROM EntryNotes WHERE entryId = @entryID {specific_ids};";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@entryID",entryID);

                if (NoteIds != null && NoteIds.Any())
                    cmd.Parameters.AddWithValue("@ids", NoteIds!.ToArray());

                var deleted_rows_count = await cmd.ExecuteNonQueryAsync();
                return deleted_rows_count;

            });
            
        }*/

        public async Task<long?> Create(Entry entry, EntryNote entry_note) {

            await using var conn = new NpgsqlConnection(DAOManager.connection_string);
            await conn.OpenAsync();

            await using var transaction = await conn.BeginTransactionAsync();

            try {

                long entry_note_ID = 0;

                // 1. Create note
                await using (var cmd = conn.CreateCommand()) {

                    cmd.Transaction = transaction;

                    cmd.CommandText = @"
                        INSERT INTO EntryNotes 
                            (entryId, money, note, date)
                        VALUES
                            (@entryId, @money, @note, @date)
                        RETURNING id
                    ";

                    cmd.Parameters.Add("@entryId", NpgsqlDbType.Bigint)
                        .Value = entry.ID;

                    cmd.Parameters.Add("@money", NpgsqlDbType.Integer)
                        .Value = (object?) entry_note.money ?? DBNull.Value;

                    cmd.Parameters.Add("@note", NpgsqlDbType.Varchar)
                        .Value = (object?) entry_note.note ?? DBNull.Value;

                    cmd.Parameters.Add("@date", NpgsqlDbType.Date)
                        .Value = entry_note.date;

                    object? result = await cmd.ExecuteScalarAsync();
                    if (result is not long id)
                        throw new Exception("Error while creating entry note");

                    entry_note_ID = id;

                }

                // 2. Updating Entry
                await using (var cmd = conn.CreateCommand()) {

                    cmd.Transaction = transaction;
                    if (await _updateEntry(cmd,entry) == false)
                        throw new Exception("Error while updating entry");
                }

                // 3. Commit
                await transaction.CommitAsync();
                return entry_note_ID;

            }
            catch (Exception ex) {
                await transaction.RollbackAsync();
                Log.Error(ex, ex.Message);
                return null;
            }

        }

        public async Task<bool> Update(Entry entry, EntryNote entry_note) {

            await using var conn = new NpgsqlConnection(DAOManager.connection_string);
            await conn.OpenAsync();

            await using var transaction = await conn.BeginTransactionAsync();

            try {

                // 1. Updating note
                await using (var cmd = conn.CreateCommand()) {

                    cmd.Transaction = transaction;

                    cmd.CommandText = @"
                        UPDATE Entries 
                        SET
                            money = @money,
                            note = @note,
                            date = @date
                        WHERE id = @id AND entryId = @entryId
                    ";

                    cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                        .Value = entry_note.ID;

                    cmd.Parameters.Add("@entryId", NpgsqlDbType.Bigint)
                        .Value = entry.ID;

                    cmd.Parameters.Add("@money", NpgsqlDbType.Integer)
                        .Value = (object?) entry_note.money ?? DBNull.Value;

                    cmd.Parameters.Add("@note", NpgsqlDbType.Varchar)
                        .Value = (object?) entry_note.note ?? DBNull.Value;

                    cmd.Parameters.Add("@date", NpgsqlDbType.Date)
                        .Value = entry_note.date;

                    var lines = await cmd.ExecuteNonQueryAsync();
                    if (lines == 0)
                        throw new Exception("Error while updating entry note");

                }

                // 2. Updating Entry
                await using (var cmd = conn.CreateCommand()) {

                    cmd.Transaction = transaction;
                    if (await _updateEntry(cmd,entry) == false)
                        throw new Exception("Error while updating entry");
                }

                // 3. Commit
                await transaction.CommitAsync();
                return true;

            }
            catch (Exception ex) {
                await transaction.RollbackAsync();
                Log.Error(ex, ex.Message);
                return false;
            }

        }

        public async Task<DAOListing<EntryNote>> Values(Query query) {
            return await DAOUtils.List("EntryNotes","id, money, note, date",query, r => _serialize(r));
        }



        private static async Task<bool> _updateEntry(NpgsqlCommand cmd, Entry entry) {
            
            cmd.CommandText = @"
                UPDATE Entries
                SET
                    status = @status,
                    lastChangeDate = @lastChangeDate,
                    moneyAmountSpent = @moneyAmountSpent,
                    moneyAmount = @moneyAmount 
                WHERE id = @id
            ";

            cmd.Parameters.Clear();

            cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                .Value = entry.ID;

            cmd.Parameters.Add("@status", NpgsqlDbType.Char)
                .Value = EntryStatusHandler.importDAO(entry.status);

            cmd.Parameters.Add("@lastChangeDate", NpgsqlDbType.Date)
                .Value = entry.last_change_date;

            cmd.Parameters.Add("@moneyAmount", NpgsqlDbType.Integer)
                .Value = entry.money;

            cmd.Parameters.Add("@moneyAmountSpent", NpgsqlDbType.Integer)
                .Value = (object?) entry.money_spent ?? DBNull.Value;

            return await cmd.ExecuteNonQueryAsync() == 1;

        }


    }

}
