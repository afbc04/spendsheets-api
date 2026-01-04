using Npgsql;
using NpgsqlTypes;
using Serilog;

namespace DAO {

    enum TagField {
        id,
        name,
        description
    };

    public class TagDAO {

        
        private static Tag _serialize(NpgsqlDataReader r) {
            return new Tag(
                r.getLong((int) TagField.id),
                r.getString((int) TagField.name),
                r.tryGetString((int) TagField.description)
            );
        }

        public async Task<Tag?> Get(long ID) {

            const string sql = @"SELECT 
                    id, name, description 
                FROM Tags WHERE id = @id;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id",ID);
                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                    return _serialize(reader);

                return null;

            });

        }

        public async Task<bool> Contains(long ID) {

            const string sql = "SELECT COUNT(*) FROM Tags WHERE id = @id;";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                var count = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                return count > 0;

            });

        }

        public async Task<bool> Delete(long ID) {

            const string sql = "DELETE FROM Tags WHERE id = @id";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                var lines = await cmd.ExecuteNonQueryAsync();
                return lines > 0;

            });

        }

        public async Task<long> ClearAll() {

            const string sql = "DELETE FROM Tags;";
            return await DAOUtils.Query(sql, async cmd => {

                var deleted_rows_count = await cmd.ExecuteNonQueryAsync();
                return deleted_rows_count;

            });
            
        }

        public async Task<long> ClearSome(IList<long> list_of_ids) {

            const string sql = @"DELETE FROM Tags WHERE id = ANY(@ids);";
            return await DAOUtils.Query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@ids", list_of_ids.ToArray());

                var deleted_rows_count = await cmd.ExecuteNonQueryAsync();
                return deleted_rows_count;

            });
            
        }

        public async Task<long?> Create(Tag tag) {

            try {

                const string sql = @"
                    INSERT INTO Tags 
                        (name, description)
                    VALUES
                        (@name, @description)
                    RETURNING id";

                return await DAOUtils.Query<long?>(sql, async cmd => {

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar)
                        .Value = tag.name;

                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar)
                        .Value = (object?) tag.description ?? DBNull.Value;

                    object? result = await cmd.ExecuteScalarAsync();
                    return result is long id ? id : null;

                });

            }
            catch (Exception ex) {
                Log.Error(ex.StackTrace ?? ex.Message);
                return null;
            }

        }

        public async Task<bool> Update(Tag tag) {

            try {

                const string sql = @"
                    UPDATE Tags
                    SET 
                        name = @name,
                        description = @description
                    WHERE id = @id";

                return await DAOUtils.Query(sql, async cmd => {

                    cmd.Parameters.Add("@id", NpgsqlDbType.Bigint)
                        .Value = tag.ID;

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar)
                        .Value = tag.name;

                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar)
                        .Value = (object?) tag.description ?? DBNull.Value;

                    await cmd.ExecuteNonQueryAsync();
                    return true;

                });

            }
            catch (Exception ex) {
                Log.Error(ex.StackTrace ?? ex.Message);
                return false;
            }

        }

        public async Task<DAOListing<Tag>> Values(Query query) {
            return await DAOUtils.List("Tags","id, name, description",query, r => _serialize(r));
        }

        public async Task<IList<long>> Keys() {

            string sql = "SELECT id FROM Tags;";

            return await DAOUtils.Query(sql, async cmd => {

                var list = new List<long>();
                await using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                    list.Add(reader.GetInt64(0));
                
                return list;

            });

        }

        public async Task<long> Size() {
            return await DAOUtils.Count("Tags");
        }

        public async Task<bool> IsEmpty() {
            return await this.Size() == 0;
        }

    }
}
