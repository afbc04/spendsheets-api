using Npgsql;
using NpgsqlTypes;
using Serilog;

namespace DAO {

    enum EntryDetailsFields {
        id,
        category_id,
        category_name,
        category_description,
        monthly_service_id,
        monthly_service_name,
        monthly_service_description,
        monthly_service_active,
        is_visible,
        type,
        money_amount,
        money_amount_spent,
        money_amount_spent_movements,
        date,
        last_change_date,
        creation_date,
        finish_date,
        due_date,
        description,
        status,
        deletion_date,
        deleted_status,
        last_status
    };

    enum EntryListFields {
        id,
        category_id,
        category_name,
        monthly_service_id,
        monthly_service_name,
        type,
        money_amount,
        money_amount_spent,
        date,
        due_date,
        status,
        deleted_status
    };

    public class EntryDAO {

        public async Task<EntryDetails?> GetDetailed(long ID) {
            return await _Get(ID,EntryGetDAO.serialize_detailed);
        }

        public async Task<EntryTransaction?> GetTransaction(long ID) {
            return await _Get(ID,EntryGetDAO.serialize_transaction);
        }

        public async Task<EntrySavings?> GetSavings(long ID) {
            return await _Get(ID,EntryGetDAO.serialize_savings);
        }

        public async Task<EntryCommitment?> GetCommitment(long ID) {
            return await _Get(ID,EntryGetDAO.serialize_commitment);
        }

        private async Task<T?> _Get<T>(long ID, Func<NpgsqlDataReader,T> serializer) where T : class {

            const string sql = @"SELECT 
                    id, categoryId, categoryName, categoryDescription, 
                    monthlyServiceId, monthlyServiceName, monthlyServiceDescription, monthlyServiceActive,
                    isVisible, type, moneyAmount, moneyAmountSpent, moneyAmountSpentMovements, date, lastChangeDate, creationDate,
                    finishDate, dueDate, description, status, deletionDate, deletedStatus, lastStatus
                FROM VEntries WHERE id = @id;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id",ID);
                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                    return serializer(reader);

                return null;

            });

        }

        public async Task<bool> Contains(long ID) {

            const string sql = "SELECT COUNT(*) FROM Entries WHERE id = @id;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                var count = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                return count > 0;

            });

        }

        public async Task<bool> Contains(long ID, EntryType type) {

            const string sql = "SELECT COUNT(*) FROM Entries WHERE id = @id AND type = @type;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                cmd.Parameters.AddWithValue("@type", EntryTypeHandler.importDAO(type));

                var count = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                return count > 0;

            });

        }

        public async Task<EntryType?> GetType(long ID) {

            const string sql = "SELECT type FROM Entries WHERE id = @id;";
            return await DAOUtils.Query<EntryType?>(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                
                object? type_extracted = await cmd.ExecuteScalarAsync();
                if (type_extracted == null || type_extracted == DBNull.Value)
                    return null;

                return EntryTypeHandler.exportDAO(Convert.ToChar(type_extracted));

            });

        }

        public async Task<bool> Delete(long ID) {

            const string sql = "DELETE FROM Entries WHERE id = @id";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                var lines = await cmd.ExecuteNonQueryAsync();
                return lines > 0;

            });

        }

        /*
        public async Task<long> Clear(IList<long>? ids) {

            string specific_ids = ids == null ? "" : "WHERE id = ANY(@ids)";
            string sql = $"DELETE FROM Categories {specific_ids};";
            return await DAOUtils.Query(sql, async cmd => {

                if (ids != null && ids.Any())
                    cmd.Parameters.AddWithValue("@ids", ids!.ToArray());

                var deleted_rows_count = await cmd.ExecuteNonQueryAsync();
                return deleted_rows_count;

            });
            
        }*/

        public async Task<long?> Create(Entry entry) {
        
            try {
                
                await using var conn = new NpgsqlConnection(DAOManager.connection_string);
                await conn.OpenAsync();

                await using var transaction = await conn.BeginTransactionAsync();

                try {

                    const string creation_sql = @"
                        INSERT INTO Entries
                            (categoryId, monthlyServiceId, isVisible, type, moneyAmount, moneyAmountSpent,
                            lastChangeDate, creationDate, finishDate, date, dueDate, description, status)
                        VALUES
                            (@categoryId, @monthlyServiceId, @isVisible, @type, @moneyAmount, @moneyAmountSpent,
                            @lastChangeDate, @creationDate, @finishDate, @date, @dueDate, @description, @status)
                        RETURNING id";

                    await using var cmd = new NpgsqlCommand(creation_sql, conn, transaction);

                    cmd.Parameters.Add("@categoryId", NpgsqlDbType.Bigint)
                        .Value = (object?) entry.category_ID ?? DBNull.Value;

                    cmd.Parameters.Add("@monthlyServiceId", NpgsqlDbType.Bigint)
                        .Value = (object?) entry.monthly_service_ID ?? DBNull.Value;

                    cmd.Parameters.Add("@isVisible", NpgsqlDbType.Boolean)
                        .Value = entry.is_visible;

                    cmd.Parameters.Add("@type", NpgsqlDbType.Char)
                        .Value = EntryTypeHandler.importDAO(entry.type);

                    cmd.Parameters.Add("@moneyAmount", NpgsqlDbType.Integer)
                        .Value = entry.money;

                    cmd.Parameters.Add("@moneyAmountSpent", NpgsqlDbType.Integer)
                        .Value = (object?) entry.money_spent ?? DBNull.Value;

                    cmd.Parameters.Add("@lastChangeDate", NpgsqlDbType.TimestampTz)
                        .Value = entry.last_change_date.ToUniversalTime();

                    cmd.Parameters.Add("@creationDate", NpgsqlDbType.Date)
                        .Value = entry.creation_date;

                    cmd.Parameters.Add("@finishDate", NpgsqlDbType.Date)
                        .Value = (object?) entry.finish_date ?? DBNull.Value;

                    cmd.Parameters.Add("@date", NpgsqlDbType.Date)
                        .Value = entry.date;

                    cmd.Parameters.Add("@dueDate", NpgsqlDbType.Date)
                        .Value = (object?) entry.due_date ?? DBNull.Value;

                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar)
                        .Value = (object?) entry.description ?? DBNull.Value;

                    cmd.Parameters.Add("@status", NpgsqlDbType.Char)
                        .Value = EntryStatusHandler.importDAO(entry.status);

                    object? result = await cmd.ExecuteScalarAsync();

                    if (result is not long id)
                        throw new Exception();

                    // If deleted entry
                    if (entry.deleted_entry_state != null) {

                        const string create_delete_sql = @"
                            INSERT INTO DeletedEntries
                                (id, deletionDate, deletedStatus, lastStatus)
                            VALUES
                                (@id, @deletionDate, @deletedStatus, @lastStatus)";

                        await using var cmd_deleted = new NpgsqlCommand(create_delete_sql, conn, transaction);

                        cmd_deleted.Parameters.Add("@id", NpgsqlDbType.Bigint)
                            .Value = id;

                        cmd_deleted.Parameters.Add("@deletionDate", NpgsqlDbType.Date)
                            .Value = entry.deleted_entry_state.deleted_date;

                        cmd_deleted.Parameters.Add("@deletedStatus", NpgsqlDbType.Char)
                            .Value = EntryStatusHandler.importDAODeleted(entry.deleted_entry_state.delete_status);

                        cmd_deleted.Parameters.Add("@lastStatus", NpgsqlDbType.Char)
                            .Value = EntryStatusHandler.importDAO(entry.deleted_entry_state.last_status);

                        await cmd_deleted.ExecuteNonQueryAsync();

                    }

                    await transaction.CommitAsync();
                    return id;

                }
                catch {
                    await transaction.RollbackAsync();
                    throw;
                }

            }
            catch (Exception ex) {
                Log.Error(ex, "Failed to create transaction entry");
                return null;
            }
        }

        public async Task<bool> Update(Entry entry) {

            try {

                await using var conn = new NpgsqlConnection(DAOManager.connection_string);
                await conn.OpenAsync();

                await using var transaction = await conn.BeginTransactionAsync();

                try {

                    const string update_sql = @"
                        UPDATE Entries
                        SET
                            categoryId = @categoryId,
                            monthlyServiceId = @monthlyServiceId,
                            isVisible = @isVisible,
                            moneyAmount = @moneyAmount,
                            moneyAmountSpent = @moneyAmountSpent,
                            lastChangeDate = @lastChangeDate,
                            creationDate = @creationDate,
                            finishDate = @finishDate,
                            date = @date,
                            dueDate = @dueDate,
                            description = @description,
                            status = @status
                        WHERE id = @id";

                    await using var cmd = new NpgsqlCommand(update_sql, conn, transaction);

                    cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                        .Value = entry.ID;

                    cmd.Parameters.Add("@categoryId", NpgsqlDbType.Bigint)
                        .Value = (object?) entry.category_ID ?? DBNull.Value;

                    cmd.Parameters.Add("@monthlyServiceId", NpgsqlDbType.Bigint)
                        .Value = (object?) entry.monthly_service_ID ?? DBNull.Value;

                    cmd.Parameters.Add("@isVisible", NpgsqlDbType.Boolean)
                        .Value = entry.is_visible;

                    cmd.Parameters.Add("@moneyAmount", NpgsqlDbType.Integer)
                        .Value = entry.money;

                    cmd.Parameters.Add("@moneyAmountSpent", NpgsqlDbType.Integer)
                        .Value = (object?) entry.money_spent ?? DBNull.Value;

                    cmd.Parameters.Add("@lastChangeDate", NpgsqlDbType.TimestampTz)
                        .Value = entry.last_change_date.ToUniversalTime();

                    cmd.Parameters.Add("@creationDate", NpgsqlDbType.Date)
                        .Value = entry.creation_date;

                    cmd.Parameters.Add("@finishDate", NpgsqlDbType.Date)
                        .Value = (object?) entry.finish_date ?? DBNull.Value;

                    cmd.Parameters.Add("@date", NpgsqlDbType.Date)
                        .Value = entry.date;

                    cmd.Parameters.Add("@dueDate", NpgsqlDbType.Date)
                        .Value = (object?) entry.due_date ?? DBNull.Value;

                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar)
                        .Value = (object?) entry.description ?? DBNull.Value;

                    cmd.Parameters.Add("@status", NpgsqlDbType.Char)
                        .Value = EntryStatusHandler.importDAO(entry.status);

                    int updated = await cmd.ExecuteNonQueryAsync();
                    if (updated == 0)
                        throw new Exception("Entry not found");

                    if (entry.deleted_entry_state != null) {

                        const string update_deleted_sql = @"
                            INSERT INTO DeletedEntries
                                (id, deletionDate, deletedStatus, lastStatus)
                            VALUES
                                (@id, @deletionDate, @deletedStatus, @lastStatus)
                            ON CONFLICT (id)
                            DO UPDATE SET
                                deletionDate = EXCLUDED.deletionDate,
                                deletedStatus = EXCLUDED.deletedStatus,
                                lastStatus = EXCLUDED.lastStatus";

                        await using var cmd_deleted = new NpgsqlCommand(update_deleted_sql, conn, transaction);

                        cmd_deleted.Parameters.Add("@id", NpgsqlDbType.Bigint)
                            .Value = entry.ID;

                        cmd_deleted.Parameters.Add("@deletionDate", NpgsqlDbType.Date)
                            .Value = entry.deleted_entry_state.deleted_date;

                        cmd_deleted.Parameters.Add("@deletedStatus", NpgsqlDbType.Char)
                            .Value = EntryStatusHandler.importDAODeleted(
                                entry.deleted_entry_state.delete_status);

                        cmd_deleted.Parameters.Add("@lastStatus", NpgsqlDbType.Char)
                            .Value = EntryStatusHandler.importDAO(
                                entry.deleted_entry_state.last_status);

                        await cmd_deleted.ExecuteNonQueryAsync();

                    }
                    else {

                        const string delete_deleted_sql =
                            "DELETE FROM DeletedEntries WHERE id = @id";

                        await using var cmd_delete = new NpgsqlCommand(delete_deleted_sql, conn, transaction);

                        cmd_delete.Parameters.Add("@id", NpgsqlDbType.Bigint)
                            .Value = entry.ID;

                        await cmd_delete.ExecuteNonQueryAsync();

                    }

                    await transaction.CommitAsync();
                    return true;

                }
                catch {
                    await transaction.RollbackAsync();
                    throw;
                }

            }
            catch (Exception ex) {
                Log.Error(ex, "Failed to update entry");
                return false;
            }
        }

        public async Task<DAOListing<EntryList>> Values(Query query) {
            return await DAOUtils.List(
                "VEntryList",
                "id, categoryId, categoryName, monthlyServiceId, monthlyServiceName, type, moneyAmount, moneyAmountSpent, date, dueDate, status, deletedStatus",
                query, 
                r => EntryGetListDAO.serialize_list(r));
        }

        public async Task<IList<long>> Keys() {

            string sql = "SELECT id FROM Entries;";

            return await DAOUtils.Query(sql, async cmd => {

                var list = new List<long>();
                await using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                    list.Add(reader.GetInt64(0));
                
                return list;

            });

        }

        public async Task<long> Size() {
            return await DAOUtils.Count("Entries");
        }

        public async Task<bool> IsEmpty() {
            return await this.Size() == 0;
        }

    }
}
