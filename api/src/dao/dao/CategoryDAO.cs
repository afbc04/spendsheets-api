using Npgsql;
using NpgsqlTypes;
using Serilog;

namespace DAO {

    enum CategoryField {
        id,
        name,
        description
    };

    public class CategoryDAO {
        
        private static Category _serialize(NpgsqlDataReader r) {
            return new Category(
                r.getLong((int) CategoryField.id),
                r.getString((int) CategoryField.name),
                r.tryGetString((int) CategoryField.description)
            );
        }

        public async Task<Category?> Get(long ID) {

            const string sql = @"
                SELECT 
                    id, name, description 
                FROM Categories WHERE id = @id;";

            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                    .Value = ID;

                await using var reader = await cmd.ExecuteReaderAsync();
                return await reader.ReadAsync() ? _serialize(reader) : null;

            });

        }

        public async Task<bool> Contains(long ID) {

            const string sql = "SELECT EXISTS (SELECT 1 FROM Categories WHERE id = @id);";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                    .Value = ID;

                return (bool) (await cmd.ExecuteScalarAsync() ?? false);

            });

        }

        public async Task<bool> Delete(long ID) {

            const string sql = "DELETE FROM Categories WHERE id = @id";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                    .Value = ID;
                
                var lines = await cmd.ExecuteNonQueryAsync();
                return lines > 0;

            });

        }

        public async Task<long> Clear(IList<long>? ids) {

            if (ids != null && ids.Any() == false)
                return 0;

            string sql = ids == null
                ? "DELETE FROM Categories;"
                : "DELETE FROM Categories WHERE id = ANY(@ids);";

            return await DAOUtils.Query(sql, async cmd => {

                if (ids != null)
                    cmd.Parameters.Add("@ids",NpgsqlDbType.Array | NpgsqlDbType.Bigint)
                        .Value = ids.ToArray();

                var deleted_rows_count = await cmd.ExecuteNonQueryAsync();
                return deleted_rows_count;

            });
            
        }

        public async Task<long?> Create(Category category) {

            try {

                const string sql = @"
                    INSERT INTO Categories 
                        (name, description)
                    VALUES
                        (@name, @description)
                    RETURNING id";

                return await DAOUtils.Query<long?>(sql, async cmd => {

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar)
                        .Value = category.name;

                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar)
                        .Value = (object?) category.description ?? DBNull.Value;

                    object? result = await cmd.ExecuteScalarAsync();
                    return result is long id ? id : null;

                });

            }
            catch (Exception ex) {
                Log.Error(ex, "Error creating category");
                return null;
            }

        }

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

                    var lines = await cmd.ExecuteNonQueryAsync();
                    return lines > 0;

                });

            }
            catch (Exception ex) {
                Log.Error(ex, "Error updating category");
                return false;
            }

        }

        public async Task<DAOListing<Category>> Values(Query query) {
            return await DAOUtils.List("Categories","id, name, description",query, r => _serialize(r));
        }

    }
}
