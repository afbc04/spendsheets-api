using Npgsql;
using NpgsqlTypes;
using Serilog;

namespace DAO {

    enum MonthlyServiceSimpleField {
        id,
        name,
        description,
        categoryRelated,
        moneyAmount,
        isActive
    };

    enum MonthlyServiceFieldList {
        id,
        name,
        isActive
    };

    enum MonthlyServiceFullField {
        id,
        name,
        description,
        moneyAmount,
        isActive,
        categoryID,
        categoryName,
        categoryDescription
    };

    public class MonthlyServiceDAO {

        private static MonthlyServiceList _serializeList(NpgsqlDataReader r) {
            return new MonthlyServiceList(
                r.getLong((int) MonthlyServiceFieldList.id),
                r.getString((int) MonthlyServiceSimpleField.name),
                r.getBool((int) MonthlyServiceFieldList.isActive)
            );
        }

        private static MonthlyServiceSimple _serializeSimple(NpgsqlDataReader r) {
            return new MonthlyServiceSimple(
                r.getLong((int) MonthlyServiceSimpleField.id),
                r.getString((int) MonthlyServiceSimpleField.name),
                r.tryGetString((int) MonthlyServiceSimpleField.description),
                r.tryGetInt((int) MonthlyServiceSimpleField.moneyAmount),
                r.getBool((int) MonthlyServiceSimpleField.isActive),
                r.tryGetLong((int) MonthlyServiceSimpleField.categoryRelated)
            );
        }

        private static MonthlyServiceFull _serializeFull(NpgsqlDataReader r) {

            long? category_id = r.tryGetLong((int) MonthlyServiceFullField.categoryID);

            return new MonthlyServiceFull(
                r.getLong((int) MonthlyServiceFullField.id),
                r.getString((int) MonthlyServiceFullField.name),
                r.tryGetString((int) MonthlyServiceFullField.description),
                r.tryGetInt((int) MonthlyServiceFullField.moneyAmount),
                r.getBool((int) MonthlyServiceFullField.isActive),
                category_id == null ? null : new Category(
                    (long) category_id!,
                    r.getString((int) MonthlyServiceFullField.categoryName),
                    r.tryGetString((int) MonthlyServiceFullField.categoryDescription)
                )
            );
        }

        public async Task<MonthlyServiceSimple?> GetSimple(long ID) {

            const string sql = @"SELECT 
                    id, name, description, categoryRelated, moneyAmount, isActive 
                FROM MonthlyServices WHERE id = @id;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id",ID);
                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                    return _serializeSimple(reader);

                return null;

            });

        }

        public async Task<MonthlyServiceFull?> GetFull(long ID) {

            const string sql = @"SELECT 
                    id, name, description, moneyAmount, isActive, categoryId, categoryName, categoryDescription
                FROM VMonthlyServices WHERE id = @id;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id",ID);
                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                    return _serializeFull(reader);

                return null;

            });

        }

        public async Task<bool> Contains(long ID) {

            const string sql = "SELECT COUNT(*) FROM MonthlyServices WHERE id = @id;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                var count = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                return count > 0;

            });

        }

        public async Task<bool> Delete(long ID) {

            const string sql = "DELETE FROM MonthlyServices WHERE id = @id";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                var lines = await cmd.ExecuteNonQueryAsync();
                return lines > 0;

            });

        }

        public async Task<long> ClearAll(bool? active) {

            string active_filter = active == null ? "" : "WHERE isActive = @active";
            string sql = $"DELETE FROM MonthlyServices {active_filter};";
            return await DAOUtils.Query(sql, async cmd => {

                if (active != null)
                    cmd.Parameters.AddWithValue("@active", active);

                var deleted_rows_count = await cmd.ExecuteNonQueryAsync();
                return deleted_rows_count;

            });
            
        }

        public async Task<long> ClearSome(IList<long> list_of_ids, bool? active) {

            string active_filter = active == null ? "" : "AND isActive = @active";
            string sql = $"DELETE FROM MonthlyServices WHERE id = ANY(@ids) {active_filter};";
            return await DAOUtils.Query(sql, async cmd => {

                if (active != null)
                    cmd.Parameters.AddWithValue("@active", active);

                cmd.Parameters.AddWithValue("@ids", list_of_ids.ToArray());

                var deleted_rows_count = await cmd.ExecuteNonQueryAsync();
                return deleted_rows_count;

            });
            
        }

        /*
        public async Task<bool> MapSome(MonthlyServiceSimple monthlyService, IList<long> list_of_ids, bool? active) {

            try {

                const string sql = @"
                    UPDATE MonthlyServices
                    SET 
                        moneyAmount = @moneyAmount,
                        isActive = @isActive
                    WHERE WHERE id = ANY(@ids) {active_filter}";

                return await DAOUtils.Query(sql, async cmd => {

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar)
                        .Value = monthlyService.name;

                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar)
                        .Value = (object?) monthlyService.description ?? DBNull.Value;

                    cmd.Parameters.Add("@categoryRelated", NpgsqlDbType.Bigint)
                        .Value = (object?) monthlyService.category_related ?? DBNull.Value;

                    cmd.Parameters.Add("@moneyAmount", NpgsqlDbType.Integer)
                        .Value = (object?) monthlyService.money_amount ?? DBNull.Value;

                    cmd.Parameters.Add("@isActive", NpgsqlDbType.Boolean)
                        .Value = monthlyService.active;

                    await cmd.ExecuteNonQueryAsync();
                    return true;

                });

            }
            catch (Exception ex) {
                Log.Error(ex.StackTrace ?? ex.Message);
                return false;
            }

        }*/

        public async Task<long?> Create(MonthlyServiceSimple monthlyService) {

            try {

                const string sql = @"
                    INSERT INTO MonthlyServices 
                        (name, description, categoryRelated, moneyAmount, isActive)
                    VALUES
                        (@name, @description, @categoryRelated, @moneyAmount, @isActive)
                    RETURNING id";

                return await DAOUtils.Query<long?>(sql, async cmd => {

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar)
                        .Value = monthlyService.name;

                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar)
                        .Value = (object?) monthlyService.description ?? DBNull.Value;

                    cmd.Parameters.Add("@categoryRelated", NpgsqlDbType.Bigint)
                        .Value = (object?) monthlyService.category_related ?? DBNull.Value;

                    cmd.Parameters.Add("@moneyAmount", NpgsqlDbType.Integer)
                        .Value = (object?) monthlyService.money_amount ?? DBNull.Value;

                    cmd.Parameters.Add("@isActive", NpgsqlDbType.Boolean)
                        .Value = monthlyService.active;

                    object? result = await cmd.ExecuteScalarAsync();
                    return result is long id ? id : null;

                });

            }
            catch (Exception ex) {
                Log.Error(ex.StackTrace ?? ex.Message);
                return null;
            }

        }

        public async Task<bool> Update(MonthlyServiceSimple monthlyService) {

            try {

                const string sql = @"
                    UPDATE MonthlyServices
                    SET 
                        name = @name,
                        description = @description,
                        categoryRelated = @categoryRelated,
                        moneyAmount = @moneyAmount,
                        isActive = @isActive
                    WHERE id = @id";

                return await DAOUtils.Query(sql, async cmd => {

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar)
                        .Value = monthlyService.name;

                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar)
                        .Value = (object?) monthlyService.description ?? DBNull.Value;

                    cmd.Parameters.Add("@categoryRelated", NpgsqlDbType.Bigint)
                        .Value = (object?) monthlyService.category_related ?? DBNull.Value;

                    cmd.Parameters.Add("@moneyAmount", NpgsqlDbType.Integer)
                        .Value = (object?) monthlyService.money_amount ?? DBNull.Value;

                    cmd.Parameters.Add("@isActive", NpgsqlDbType.Boolean)
                        .Value = monthlyService.active;

                    await cmd.ExecuteNonQueryAsync();
                    return true;

                });

            }
            catch (Exception ex) {
                Log.Error(ex.StackTrace ?? ex.Message);
                return false;
            }

        }

        public async Task<DAOListing<MonthlyServiceList>> Values(Query query) {
            return await DAOUtils.List("MonthlyServices","id, name, isActive",query, r => _serializeList(r));
        }

        public async Task<IList<long>> Keys() {

            string sql = "SELECT id FROM MonthlyServices;";

            return await DAOUtils.Query(sql, async cmd => {

                var list = new List<long>();
                await using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                    list.Add(reader.GetInt64(0));
                
                return list;

            });

        }

        public async Task<long> Size() {
            return await DAOUtils.Count("MonthlyServices");
        }

        public async Task<bool> IsEmpty() {
            return await this.Size() == 0;
        }

    }
}
