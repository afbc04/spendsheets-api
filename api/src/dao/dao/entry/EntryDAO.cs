using Npgsql;
using NpgsqlTypes;
using Serilog;

namespace DAO {

    enum EntryDetailsFields {
        id,
        category_id,
        category_name,
        category_description,
        collection_id,
        collection_name,
        collection_description,
        collection_is_monthly_service,
        is_visible,
        is_public,
        is_active,
        type,
        money_amount,
        money_amount_spent,
        date,
        last_change_date,
        creation_date,
        finish_date,
        due_date,
        description,
        status,
        last_status,
        deletion_date
    };

    enum EntryListFields {
        id,
        category_id,
        category_name,
        collection_id,
        collection_name,
        collection_is_monthly_service,
        is_visible,
        is_public,
        is_active,
        type,
        money_amount,
        money_amount_spent,
        date,
        last_change_date,
        due_date,
        status
    };

    public class EntryDAO {

        private static Entry _serialize(NpgsqlDataReader r) =>
            new(
                r.getLong((int) EntryDetailsFields.id),
                r.tryGetLong((int) EntryDetailsFields.category_id),
                r.tryGetLong((int) EntryDetailsFields.collection_id),
                r.getBool((int) EntryDetailsFields.is_visible),
                r.getBool((int) EntryDetailsFields.is_public),
                r.getBool((int) EntryDetailsFields.is_active),
                EntryTypeHandler.exportDAO(r.getChar((int) EntryDetailsFields.type)),
                r.getInt((int) EntryDetailsFields.money_amount),
                r.tryGetInt((int) EntryDetailsFields.money_amount_spent),
                r.getDate((int) EntryDetailsFields.date),
                r.getDateTime((int) EntryDetailsFields.last_change_date),
                r.getDate((int) EntryDetailsFields.creation_date),
                r.tryGetDate((int) EntryDetailsFields.finish_date),
                r.tryGetDate((int) EntryDetailsFields.due_date),
                r.tryGetString((int) EntryDetailsFields.description),
                EntryStatusHandler.exportDAO(r.getChar((int) EntryDetailsFields.status)),
                EntryStatusHandler.exportDAO(r.tryGetChar((int) EntryDetailsFields.last_status) ?? '0'),
                r.tryGetDate((int) EntryDetailsFields.deletion_date)
            );

        private static EntryList _serialize_list(NpgsqlDataReader r) =>
            new(
                r.getLong((int) EntryListFields.id),

                r.tryGetLong((int) EntryListFields.category_id),
                r.tryGetString((int) EntryListFields.category_name),

                r.tryGetLong((int) EntryListFields.collection_id),
                r.tryGetString((int) EntryListFields.collection_name),
                r.tryGetBool((int) EntryListFields.collection_is_monthly_service),

                r.getBool((int) EntryListFields.is_visible),
                r.getBool((int) EntryListFields.is_public),
                r.getBool((int) EntryListFields.is_active),
                EntryTypeHandler.exportDAO(r.getChar((int) EntryListFields.type)),
                r.getInt((int) EntryListFields.money_amount),
                r.tryGetInt((int) EntryListFields.money_amount_spent),
                r.getDate((int) EntryListFields.date),
                r.getDateTime((int) EntryListFields.last_change_date),
                r.tryGetDate((int) EntryListFields.due_date),
                EntryStatusHandler.exportDAO(r.getChar((int) EntryListFields.status))
            );

        private static Category? _serialize_category_of_entry(NpgsqlDataReader r) {

            Category? category = null;
            long? category_id = r.tryGetLong((int) EntryDetailsFields.category_id);
            
            if (category_id != null)
                category = new Category(
                    (long) category_id,
                    r.getString((int) EntryDetailsFields.category_name),
                    r.tryGetString((int) EntryDetailsFields.category_description)
                );

            return category;

        }

        private static Collection? _serialize_collection_of_entry(NpgsqlDataReader r) {

            Collection? collection = null;
            long? collection_id = r.tryGetLong((int) EntryDetailsFields.collection_id);
            
            if (collection_id != null)
                collection = new(
                    (long) collection_id,
                    r.getString((int) EntryDetailsFields.collection_name),
                    r.tryGetString((int) EntryDetailsFields.collection_description),
                    null,
                    null,
                    r.getBool((int) EntryDetailsFields.collection_is_monthly_service),
                    null
                );

            return collection;

        }

        public async Task<EntryDetails?> GetDetailed(long ID) {

            const string sql = @"SELECT 
                    id, categoryId, categoryName, categoryDescription, 
                    collectionId, collectionName, collectionDescription, collectionIsMonthlyService,
                    isVisible, isPublic, isActive, type, moneyAmount, moneyAmountSpent,
                    date, lastChangeDate, creationDate, finishDate, dueDate, description, 
                    status, lastStatus, deletionDate
                FROM VEntries WHERE id = @id;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id",ID);
                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync()) {

                    Entry? entry = _serialize(reader);
                    Category? category = _serialize_category_of_entry(reader);
                    Collection? collection = _serialize_collection_of_entry(reader);

                    return entry != null 
                        ? new EntryDetails(entry,category,collection)
                        : null; 

                }

                return null;

            });

        }

        public async Task<Entry?> Get(long ID) {

            const string sql = @"SELECT 
                    id, categoryId, categoryName, categoryDescription, 
                    collectionId, collectionName, collectionDescription, collectionIsMonthlyService,
                    isVisible, isPublic, isActive, type, moneyAmount, moneyAmountSpent,
                    date, lastChangeDate, creationDate, finishDate, dueDate, description, 
                    status, lastStatus, deletionDate
                FROM VEntries WHERE id = @id;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id",ID);
                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                    return _serialize(reader);

                return null;

            });

        }

        public async Task<bool> Contains(long ID) {

            const string sql = "SELECT EXISTS (SELECT 1 FROM Entries WHERE id = @id);";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                    .Value = ID;

                return (bool) (await cmd.ExecuteScalarAsync() ?? false);

            });

        }

        public async Task<bool> Delete(long ID) {

            const string sql = "DELETE FROM Entries WHERE id = @id";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                    .Value = ID;

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
                            (categoryId, collectionId, isVisible, isPublic, isActive, type, moneyAmount,
                            moneyAmountSpent, lastChangeDate, creationDate, finishDate, date, dueDate, 
                            description, status, lastStatus, deletionDate)
                        VALUES
                            (@categoryId, @collectionId, @isVisible, @isPublic, @isActive, @type, @moneyAmount,
                            @moneyAmountSpent, @lastChangeDate, @creationDate, @finishDate, @date, @dueDate, 
                            @description, @status, @lastStatus, @deletionDate)
                        RETURNING id";

                    await using var cmd = new NpgsqlCommand(creation_sql, conn, transaction);

                    cmd.Parameters.Add("@categoryId", NpgsqlDbType.Bigint)
                        .Value = (object?) entry.category_ID ?? DBNull.Value;

                    cmd.Parameters.Add("@collectionId", NpgsqlDbType.Bigint)
                        .Value = (object?) entry.collection_ID ?? DBNull.Value;

                    cmd.Parameters.Add("@isVisible", NpgsqlDbType.Boolean)
                        .Value = entry.is_visible;

                    cmd.Parameters.Add("@isPublic", NpgsqlDbType.Boolean)
                        .Value = entry.is_public;

                    cmd.Parameters.Add("@isActive", NpgsqlDbType.Boolean)
                        .Value = entry.active;

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

                    cmd.Parameters.Add("@lastStatus", NpgsqlDbType.Char)
                        .Value = entry.last_status == null ? DBNull.Value : (object?) EntryStatusHandler.importDAO((EntryStatus) entry.last_status);

                    cmd.Parameters.Add("@deletionDate", NpgsqlDbType.Date)
                        .Value = (object?) entry.deleted_date ?? DBNull.Value;

                    object? result = await cmd.ExecuteScalarAsync();

                    if (result is not long id)
                        throw new Exception();

                    await transaction.CommitAsync();
                    return id;

                }
                catch {
                    await transaction.RollbackAsync();
                    throw;
                }

            }
            catch (Exception ex) {
                Log.Error(ex, "Failed to create entry");
                return null;
            }
        }

        public async Task<bool> Update(Entry entry) {

            try {

                const string sql = @"
                    UPDATE Entries
                    SET
                        categoryId = @categoryId,
                        collectionId = @collectionId,
                        isVisible = @isVisible,
                        isPublic = @isPublic,
                        isActive = @isActive,
                        type = @type,
                        moneyAmount = @moneyAmount,
                        moneyAmountSpent = @moneyAmountSpent,
                        lastChangeDate = @lastChangeDate,
                        creationDate = @creationDate,
                        finishDate = @finishDate,
                        date = @date,
                        dueDate = @dueDate,
                        description = @description,
                        status = @status,
                        lastStatus = @lastStatus,
                        deletionDate = @deletionDate
                    WHERE id = @id";

                return await DAOUtils.Query(sql, async cmd => {

                    cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                        .Value = entry.ID;

                    cmd.Parameters.Add("@categoryId", NpgsqlDbType.Bigint)
                        .Value = (object?) entry.category_ID ?? DBNull.Value;

                    cmd.Parameters.Add("@collectionId", NpgsqlDbType.Bigint)
                        .Value = (object?) entry.collection_ID ?? DBNull.Value;

                    cmd.Parameters.Add("@isVisible", NpgsqlDbType.Boolean)
                        .Value = entry.is_visible;

                    cmd.Parameters.Add("@isPublic", NpgsqlDbType.Boolean)
                        .Value = entry.is_public;

                    cmd.Parameters.Add("@isActive", NpgsqlDbType.Boolean)
                        .Value = entry.active;

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

                    cmd.Parameters.Add("@lastStatus", NpgsqlDbType.Char)
                        .Value = entry.last_status == null ? DBNull.Value : (object?) EntryStatusHandler.importDAO((EntryStatus) entry.last_status);

                    cmd.Parameters.Add("@deletionDate", NpgsqlDbType.Date)
                        .Value = (object?) entry.deleted_date ?? DBNull.Value;

                    await cmd.ExecuteNonQueryAsync();
                    return true;

                });

            }
            catch (Exception ex) {
                Log.Error(ex, "Error updating entry");
                return false;
            }
        }

        public async Task<DAOListing<EntryList>> Values(Query query) {
            return await DAOUtils.List(
                "VEntryList",
                "id, categoryId, categoryName, collectionId, collectionName, collectionIsMonthlyService, isVisible, isPublic, isActive, type, moneyAmount, moneyAmountSpent, date, lastChangeDate, dueDate, status",
                query, 
                r => _serialize_list(r));
        }

    }
}
