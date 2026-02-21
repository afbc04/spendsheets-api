using Npgsql;
using NpgsqlTypes;
using Serilog;
using DTO;

namespace DAO {

    enum CollectionField {
        id,
        name,
        description,
        isMonthlyService,
        categoryRelated,
        moneyAmount,
        isMonthlyServiceActive
    };

    enum CollectionListField {
        id,
        name,
        isMonthlyService,
        isMonthlyServiceActive
    };

    enum CollectionDetailsField {
        id,
        name,
        description,
        isMonthlyService,
        moneyAmount,
        isMonthlyServiceActive,
        categoryID,
        categoryName,
        categoryDescription
    };

    public class CollectionDAO {

        private static CollectionList _serializeList(NpgsqlDataReader r) =>
            new(
                r.getLong((int) CollectionListField.id),
                r.getString((int) CollectionListField.name),
                r.getBool((int) CollectionListField.isMonthlyService),
                r.tryGetBool((int) CollectionListField.isMonthlyServiceActive)
            );

        private static Collection _serialize(NpgsqlDataReader r) =>
            new(
                r.getLong((int) CollectionField.id),
                r.getString((int) CollectionField.name),
                r.tryGetString((int) CollectionField.description),
                r.tryGetInt((int) CollectionField.moneyAmount),
                r.tryGetLong((int) CollectionField.categoryRelated),
                r.getBool((int) CollectionField.isMonthlyService),
                r.tryGetBool((int) CollectionField.isMonthlyServiceActive)
            );

        private static CollectionDetails _serializeDetails(NpgsqlDataReader r) {

            long? category_id = r.tryGetLong((int) CollectionDetailsField.categoryID);

            var collection = new Collection(
                r.getLong((int) CollectionDetailsField.id),
                r.getString((int) CollectionDetailsField.name),
                r.tryGetString((int) CollectionDetailsField.description),
                r.tryGetInt((int) CollectionDetailsField.moneyAmount),
                null,
                r.getBool((int) CollectionDetailsField.isMonthlyService),
                r.tryGetBool((int) CollectionDetailsField.isMonthlyServiceActive)
            );

            return new(
                collection,
                category_id == null 
                    ? null 
                    : new Category(
                        (long) category_id!,
                        r.getString((int) CollectionDetailsField.categoryName),
                        r.tryGetString((int) CollectionDetailsField.categoryDescription)
                    )
            );

        }

        public async Task<Collection?> Get(long ID) {

            const string sql = @"
                SELECT 
                    id, name, description, isMonthlyService, categoryRelated, moneyAmount, isMonthlyServiceActive 
                FROM Collections WHERE id = @id;";

            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                    .Value = ID;

                await using var reader = await cmd.ExecuteReaderAsync();
                return await reader.ReadAsync() ? _serialize(reader) : null;

            });

        }

        public async Task<CollectionDetails?> GetDetailed(long ID) {

            const string sql = @"
                SELECT 
                    id, name, description, isMonthlyService, moneyAmount, isMonthlyServiceActive, categoryId, categoryName, categoryDescription
                FROM VCollections WHERE id = @id;";

            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                    .Value = ID;

                await using var reader = await cmd.ExecuteReaderAsync();
                return await reader.ReadAsync() ? _serializeDetails(reader) : null;

            });

        }

        public async Task<bool> Contains(long ID) {

            const string sql = "SELECT EXISTS (SELECT 1 FROM Collections WHERE id = @id);";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                    .Value = ID;

                return (bool) (await cmd.ExecuteScalarAsync() ?? false);

            });

        }

        public async Task<bool> Delete(long ID) {

            const string sql = "DELETE FROM Collections WHERE id = @id";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                    .Value = ID;

                var lines = await cmd.ExecuteNonQueryAsync();
                return lines > 0;

            });

        }

        public async Task<long> Clear(IList<long>? ids, bool? is_monthly_service, bool? monthly_service_active) {

            if (ids != null && ids.Any() == false)
                return 0;

            var where_sql = new List<string>();
            string sql = ids == null
                ? "DELETE FROM Collections;"
                : "DELETE FROM Collections WHERE id = ANY(@ids);";

            if (monthly_service_active != null)
                where_sql.Add("isMonthlyServiceActive = @isMonthlyServiceActive");

            if (is_monthly_service != null)
                where_sql.Add("isMonthlyService = @isMonthlyService");

            if (where_sql.Count != 0)
                sql += " WHERE " + string.Join(" AND ", where_sql);

            sql += ";";

            return await DAOUtils.Query(sql, async cmd => {

                if (ids != null)
                    cmd.Parameters.Add("@ids",NpgsqlDbType.Array | NpgsqlDbType.Bigint)
                        .Value = ids.ToArray();

                if (monthly_service_active != null)
                    cmd.Parameters.Add("@isMonthlyServiceActive", NpgsqlDbType.Boolean)
                        .Value = monthly_service_active;

                if (is_monthly_service != null)
                    cmd.Parameters.Add("@isMonthlyService", NpgsqlDbType.Boolean)
                        .Value = is_monthly_service;

                return await cmd.ExecuteNonQueryAsync();

            });
        }

        public async Task<long> Map(CollectionMapDTO collection, IList<long>? ids, bool? monthly_service_active) {

            // Update Statements
            var update_statements = new List<string>();

            if (collection.fields_changed[(int) CollectionMapDTOFields.isMonthlyServiceActive])
                update_statements.Add("isMonthlyServiceActive = @isMonthlyServiceActive");

            if (!update_statements.Any())
                return 0;

            // Filter Statements
            var where_clauses = new List<string>
            {
                "isMonthlyService = true"
            };

            if (ids != null)
                where_clauses.Add("id = ANY(@ids)");

            if (monthly_service_active != null)
                where_clauses.Add("isMonthlyServiceActive = @filterIsMonthlyServiceActive");

            var where_sql = where_clauses.Any() ? "WHERE " + string.Join(" AND ", where_clauses) : "";

            // Prepare SQL
            string sql = $@"
                UPDATE Collections
                SET 
                    {string.Join(", ", update_statements)}
                {where_sql};";

            return await DAOUtils.Query(sql, async cmd => {

                if (collection.fields_changed[(int) CollectionMapDTOFields.isMonthlyServiceActive])
                    cmd.Parameters.Add("@isMonthlyServiceActive", NpgsqlDbType.Boolean)
                        .Value = collection.is_active;

                if (ids != null)
                    cmd.Parameters.Add("@ids", NpgsqlDbType.Array | NpgsqlDbType.Bigint)
                        .Value = ids.ToArray();

                if (monthly_service_active != null)
                    cmd.Parameters.Add("@filterIsMonthlyServiceActive", NpgsqlDbType.Boolean)
                        .Value = monthly_service_active;

                return await cmd.ExecuteNonQueryAsync();

            });

        }

        public async Task<long?> Create(Collection collection) {

            try {

                const string sql = @"
                    INSERT INTO Collections 
                        (name, description, isMonthlyService, categoryRelated, moneyAmount, isMonthlyServiceActive)
                    VALUES
                        (@name, @description, @isMonthlyService, @categoryRelated, @moneyAmount, @isMonthlyServiceActive)
                    RETURNING id";

                return await DAOUtils.Query<long?>(sql, async cmd => {

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar)
                        .Value = collection.name;

                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar)
                        .Value = (object?) collection.description ?? DBNull.Value;

                    cmd.Parameters.Add("@isMonthlyService", NpgsqlDbType.Boolean)
                        .Value = collection.is_monthly_service;

                    cmd.Parameters.Add("@categoryRelated", NpgsqlDbType.Bigint)
                        .Value = (object?) collection.category_ID ?? DBNull.Value;

                    cmd.Parameters.Add("@moneyAmount", NpgsqlDbType.Integer)
                        .Value = (object?) collection.money_amount ?? DBNull.Value;

                    cmd.Parameters.Add("@isMonthlyServiceActive", NpgsqlDbType.Boolean)
                        .Value = (object?) collection.is_monthly_service_active ?? DBNull.Value;

                    object? result = await cmd.ExecuteScalarAsync();
                    return result is long id ? id : null;

                });

            }
            catch (Exception ex) {
                Log.Error(ex, "Error creating collection");
                return null;
            }

        }

        public async Task<bool> Update(Collection collection) {

            try {

                const string sql = @"
                    UPDATE Collections
                    SET 
                        name = @name,
                        description = @description,
                        isMonthlyService = @isMonthlyService,
                        categoryRelated = @categoryRelated,
                        moneyAmount = @moneyAmount,
                        isMonthlyServiceActive = @isMonthlyServiceActive
                    WHERE id = @id";

                return await DAOUtils.Query(sql, async cmd => {

                    cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                        .Value = collection.ID;

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar)
                        .Value = collection.name;

                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar)
                        .Value = (object?) collection.description ?? DBNull.Value;

                    cmd.Parameters.Add("@isMonthlyService", NpgsqlDbType.Boolean)
                        .Value = collection.is_monthly_service;

                    cmd.Parameters.Add("@categoryRelated", NpgsqlDbType.Bigint)
                        .Value = (object?) collection.category_ID ?? DBNull.Value;

                    cmd.Parameters.Add("@moneyAmount", NpgsqlDbType.Integer)
                        .Value = (object?) collection.money_amount ?? DBNull.Value;

                    cmd.Parameters.Add("@isMonthlyServiceActive", NpgsqlDbType.Boolean)
                        .Value = (object?) collection.is_monthly_service_active ?? DBNull.Value;

                    await cmd.ExecuteNonQueryAsync();
                    return true;

                });

            }
            catch (Exception ex) {
                Log.Error(ex, "Error updating collection");
                return false;
            }

        }

        public async Task<DAOListing<CollectionList>> Values(Query query) {
            return await DAOUtils.List("Collections","id, name, isMonthlyService, isMonthlyServiceActive",query, r => _serializeList(r));
        }

    }
}
