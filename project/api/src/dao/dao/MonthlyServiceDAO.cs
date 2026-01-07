using Npgsql;
using NpgsqlTypes;
using Serilog;
using DTO;

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

        public async Task<long> Clear(IList<long>? ids, bool? active) {

            var where_sql = new List<string>();
            string sql = "DELETE FROM MonthlyServices";

            if (ids != null && ids.Any())
                where_sql.Add("id = ANY(@ids)");

            if (active != null)
                where_sql.Add("isActive = @active");

            if (where_sql.Any())
                sql += " WHERE " + string.Join(" AND ", where_sql);

            sql += ";";

            return await DAOUtils.Query(sql, async cmd => {

                if (ids != null && ids.Any())
                    cmd.Parameters.AddWithValue("@ids", ids.ToArray());

                if (active != null)
                    cmd.Parameters.AddWithValue("@active", active);

                return await cmd.ExecuteNonQueryAsync();

            });
        }

        public async Task<long> Map(MonthlyServiceMapDTO monthlyService, IList<long>? ids, bool? is_active) {

            // Update Statements
            var update_statements = new List<string>();

            if (monthlyService.fields_changed[(int)MonthlyServiceMapDTOFields.moneyAmount])
                update_statements.Add("moneyAmount = @moneyAmount");

            if (monthlyService.fields_changed[(int)MonthlyServiceMapDTOFields.active])
                update_statements.Add("isActive = @isActive");

            if (!update_statements.Any())
                return 0;

            // Filter Statements
            var where_clauses = new List<string>();

            if (ids != null)
                where_clauses.Add("id = ANY(@ids)");

            if (is_active != null)
                where_clauses.Add("isActive = @filterIsActive");

            var where_sql = where_clauses.Any() ? "WHERE " + string.Join(" AND ", where_clauses) : "";

            // Prepare SQL
            string sql = $@"
                UPDATE MonthlyServices
                SET 
                    {string.Join(", ", update_statements)}
                {where_sql};";

            return await DAOUtils.Query(sql, async cmd => {

                if (monthlyService.fields_changed[(int) MonthlyServiceMapDTOFields.moneyAmount])
                    cmd.Parameters.Add("@moneyAmount", NpgsqlDbType.Integer)
                        .Value = (object?) monthlyService.money_amount ?? DBNull.Value;

                if (monthlyService.fields_changed[(int) MonthlyServiceMapDTOFields.active])
                    cmd.Parameters.Add("@isActive", NpgsqlDbType.Boolean)
                        .Value = monthlyService.is_active;

                if (ids != null)
                    cmd.Parameters.Add("@ids", NpgsqlDbType.Array | NpgsqlDbType.Bigint)
                        .Value = ids.ToArray();

                if (is_active != null)
                    cmd.Parameters.Add("@filterIsActive", NpgsqlDbType.Boolean)
                        .Value = is_active;

                return await cmd.ExecuteNonQueryAsync();

            });

        }

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

                    cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                        .Value = monthlyService.ID;

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
