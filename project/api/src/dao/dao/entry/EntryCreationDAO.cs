/*using DAO;
using Npgsql;
using NpgsqlTypes;
using Serilog;

public static class EntryCreationDAO {

    public static async Task<long?> CreateTransactionEntry(EntryTransaction entry) {
    
        try {
            
            await using var conn = new NpgsqlConnection(DAOManager.connection_string);
            await conn.OpenAsync();

            await using var transaction = await conn.BeginTransactionAsync();

            try {

                const string creation_sql = @"
                    INSERT INTO Entries
                        (categoryId, isVisible, type, moneyAmount, lastChangeDate,
                        creationDate, finishDate, date, description, status)
                    VALUES
                        (@categoryId, @isVisible, @type, @moneyAmount, @lastChangeDate,
                        @creationDate, @finishDate, @date, @description, @status)
                    RETURNING id";

                await using var cmd = new NpgsqlCommand(creation_sql, conn, transaction);

                cmd.Parameters.Add("@categoryId", NpgsqlDbType.Bigint)
                    .Value = (object?) entry.category_ID ?? DBNull.Value;

                cmd.Parameters.Add("@isVisible", NpgsqlDbType.Boolean)
                    .Value = entry.is_visible;

                cmd.Parameters.Add("@type", NpgsqlDbType.Char)
                    .Value = EntryTypeHandler.importDAO(entry.type);

                cmd.Parameters.Add("@moneyAmount", NpgsqlDbType.Integer)
                    .Value = entry.money;

                cmd.Parameters.Add("@lastChangeDate", NpgsqlDbType.TimestampTz)
                    .Value = entry.last_change_date.ToUniversalTime();

                cmd.Parameters.Add("@creationDate", NpgsqlDbType.Date)
                    .Value = entry.creation_date;

                cmd.Parameters.Add("@finishDate", NpgsqlDbType.Date)
                    .Value = (object?) entry.finish_date ?? DBNull.Value;

                cmd.Parameters.Add("@date", NpgsqlDbType.Date)
                    .Value = entry.date;

                cmd.Parameters.Add("@description", NpgsqlDbType.Varchar)
                    .Value = (object?) entry.description ?? DBNull.Value;

                cmd.Parameters.Add("@status", NpgsqlDbType.Char)
                    .Value = EntryStatusHandler.importDAO(entry.status);

                object? result = await cmd.ExecuteScalarAsync();

                if (result is not long id)
                    throw new Exception();

                // If deleted entry
                if (entry.deleted_entry != null) {

                    const string create_delete_sql = @"
                        INSERT INTO DeletedEntries
                            (id, deletionDate, deletedStatus, lastStatus)
                        VALUES
                            (@id, @deletionDate, @deletedStatus, @lastStatus)";

                    await using var cmd_deleted = new NpgsqlCommand(create_delete_sql, conn, transaction);

                    cmd_deleted.Parameters.Add("@id", NpgsqlDbType.Bigint)
                        .Value = id;

                    cmd_deleted.Parameters.Add("@deletionDate", NpgsqlDbType.Date)
                        .Value = entry.deleted_entry.deleted_date;

                    cmd_deleted.Parameters.Add("@deletedStatus", NpgsqlDbType.Char)
                        .Value = EntryStatusHandler.importDAODeleted(entry.deleted_entry.status);

                    cmd_deleted.Parameters.Add("@lastStatus", NpgsqlDbType.Char)
                        .Value = EntryStatusHandler.importDAO(entry.deleted_entry.last_status);

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

}*/
