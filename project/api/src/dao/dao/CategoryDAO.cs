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

            const string sql = @"SELECT 
                    id, name, description 
                FROM Categories WHERE id = @id;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id",ID);
                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                    return _serialize(reader);

                return null;

            });

        }

        public async Task<bool> Contains(long ID) {

            const string sql = "SELECT COUNT(*) FROM Categories WHERE id = @id;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                var count = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                return count > 0;

            });

        }

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
                Log.Error(ex.StackTrace ?? ex.Message);
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
        }

    }
}
