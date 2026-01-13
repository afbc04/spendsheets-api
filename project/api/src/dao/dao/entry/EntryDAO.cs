using Npgsql;
using NpgsqlTypes;
using Serilog;

namespace DAO {

    enum EntryDetailsFields {
        id,
        category_id,
        category_name,
        category_description,
        is_visible,
        type,
        money_amount,
        money_amount_left,
        date,
        last_change_date,
        creation_date,
        finish_date,
        description,
        status,
        deletion_date,
        deleted_status,
        last_status,
        monthly_service_id,
        monthly_service_name,
        monthly_service_description,
        monthly_service_active,
        is_generated_by_system,
        scheduled_due_date,
        real_due_date,
        tags_count
    };

    public class EntryDAO {

        public async Task<EntryDetails?> GetDetailed(long ID) {
            return await _Get(ID,EntryGetDAO.serialize_detailed);
        }

        public async Task<EntryTransaction?> GetTransaction(long ID) {
            return await _Get(ID,EntryGetDAO.serialize_transaction);
        }

        private async Task<T?> _Get<T>(long ID, Func<NpgsqlDataReader,T> serializer) where T : class {

            const string sql = @"SELECT 
                    id, categoryId, categoryName, categoryDescription, isVisible, type,
                    moneyAmount, moneyAmountLeft, date, lastChangeDate, creationDate,
                    finishDate, description, status, deletionDate, deletedStatus, lastStatus,
                    monthlyServiceId, monthlyServiceName, monthlyServiceDescription, monthlyServiceActive,
                    isGeneratedBySystem, scheduledDueDate, realDueDate, tagsCount
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

        /*
        public async Task<bool> Delete(long ID) {

            const string sql = "DELETE FROM Categories WHERE id = @id";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                var lines = await cmd.ExecuteNonQueryAsync();
                return lines > 0;

            });

        }

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

        public async Task<long?> Create(EntryTransaction entry) {
            return await EntryCreationDAO.CreateTransactionEntry(entry);
        }

        /*
        public async Task<bool> Update(Category category) {

            try {

                const string sql = @"
                    UPDATE Categories
                    SET 
                        name = @name,
                        description = @description
                    WHERE id = @id";

                return await DAOUtils.Query(sql, async cmd => {

                    cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                        .Value = category.ID;

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar)
                        .Value = category.name;

                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar)
                        .Value = (object?) category.description ?? DBNull.Value;

                    await cmd.ExecuteNonQueryAsync();
                    return true;

                });

            }
            catch (Exception ex) {
                Log.Error(ex.StackTrace ?? ex.Message);
                return false;
            }

        }

        public async Task<DAOListing<Category>> Values(Query query) {
            return await DAOUtils.List("Categories","id, name, description",query, r => _serialize(r));
        }

        public async Task<IList<long>> Keys() {

            string sql = "SELECT id FROM Categories;";

            return await DAOUtils.Query(sql, async cmd => {

                var list = new List<long>();
                await using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                    list.Add(reader.GetInt64(0));
                
                return list;

            });

        }

        public async Task<long> Size() {
            return await DAOUtils.Count("Categories");
        }

        public async Task<bool> IsEmpty() {
            return await this.Size() == 0;
        }*/

    }
}
