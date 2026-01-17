using Npgsql;
using NpgsqlTypes;
using Serilog;
using DTO;

namespace DAO {

    enum EntryMovementsField {
        id,
        money,
        comment,
        date
    };

    public class EntryMovementsDAO {

        private static EntryMovement _serialize(NpgsqlDataReader r) {
            return new EntryMovement(
                r.getLong((int) EntryMovementsField.id),
                r.GetInt32((int) EntryMovementsField.money),
                r.getDate((int) EntryMovementsField.date),
                r.tryGetString((int) EntryMovementsField.comment)
            );
        }

        public async Task<EntryMovement?> Get(long entryID, long ID) {

            const string sql = @"SELECT 
                    id, money, comment, date
                FROM EntryMovements WHERE id = @id AND entryId = @entryID;";
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

            const string sql = "SELECT COUNT(*) FROM EntryMovements WHERE id = @id AND entryId = @entryID;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                cmd.Parameters.AddWithValue("@entryID",entryID);
                var count = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                return count > 0;

            });

        }

        public async Task<bool> Delete(long entryID, long ID) {

            const string sql = "DELETE FROM EntryMovements WHERE id = @id AND entryId = @entryID";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                cmd.Parameters.AddWithValue("@entryID",entryID);
                var lines = await cmd.ExecuteNonQueryAsync();
                return lines > 0;

            });

        }

        public async Task<long> Clear(long entryID, IList<long>? MovementIds) {

            string specific_ids = MovementIds == null ? "" : "AND id = ANY(@ids)";
            string sql = $"DELETE FROM EntryMovements WHERE entryId = @entryID {specific_ids};";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@entryID",entryID);

                if (MovementIds != null && MovementIds.Any())
                    cmd.Parameters.AddWithValue("@ids", MovementIds!.ToArray());

                var deleted_rows_count = await cmd.ExecuteNonQueryAsync();
                return deleted_rows_count;

            });
            
        }

        public async Task<long?> Create(long entryID, EntryMovement entry_movement) {

            try {

                const string sql = @"
                    INSERT INTO EntryMovements 
                        (entryId, money, comment, date)
                    VALUES
                        (@entryId, @money, @comment, @date)
                    RETURNING id";

                return await DAOUtils.Query<long?>(sql, async cmd => {

                    cmd.Parameters.Add("@entryId", NpgsqlDbType.Bigint)
                        .Value = entryID;

                    cmd.Parameters.Add("@money", NpgsqlDbType.Integer)
                        .Value = entry_movement.money;

                    cmd.Parameters.Add("@comment", NpgsqlDbType.Varchar)
                        .Value = (object?) entry_movement.comment ?? DBNull.Value;

                    cmd.Parameters.Add("@date", NpgsqlDbType.Date)
                        .Value = entry_movement.date;

                    object? result = await cmd.ExecuteScalarAsync();
                    return result is long id ? id : null;

                });

            }
            catch (Exception ex) {
                Log.Error(ex.StackTrace ?? ex.Message);
                return null;
            }

        }

        public async Task<bool> Update(long entryID, EntryMovement entry_movement) {

            try {

                const string sql = @"
                    UPDATE EntryMovements
                    SET 
                        money = @money,
                        comment = @comment,
                        date = @date
                    WHERE id = @id AND entryId = @entryId";

                return await DAOUtils.Query(sql, async cmd => {

                    cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                        .Value = entry_movement.ID;

                    cmd.Parameters.Add("@entryId", NpgsqlDbType.Bigint)
                        .Value = entryID;

                    cmd.Parameters.Add("@money", NpgsqlDbType.Integer)
                        .Value = entry_movement.money;

                    cmd.Parameters.Add("@comment", NpgsqlDbType.Varchar)
                        .Value = (object?) entry_movement.comment ?? DBNull.Value;

                    cmd.Parameters.Add("@date", NpgsqlDbType.Date)
                        .Value = entry_movement.date;

                    await cmd.ExecuteNonQueryAsync();
                    return true;

                });

            }
            catch (Exception ex) {
                Log.Error(ex.StackTrace ?? ex.Message);
                return false;
            }

        }

        public async Task<DAOListing<EntryMovement>> Values(Query query) {
            return await DAOUtils.List("EntryMovements","id, money, comment, date",query, r => _serialize(r));
        }

        public async Task<IList<long>> Keys(long entryID) {

            string sql = "SELECT id FROM EntryMovements WHERE entryId = @entryId;";

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
            return await DAOUtils.Count("EntryMovements");
        }

        public async Task<bool> IsEmpty() {
            return await this.Size() == 0;
        }

    }
}
