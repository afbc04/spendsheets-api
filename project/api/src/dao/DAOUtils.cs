using Npgsql;
using Queries;

namespace DAO {

    public static class DAOReader {

        public static int getInt(this NpgsqlDataReader r, int index)
            => r.GetInt32(index);

        public static long getLong(this NpgsqlDataReader r, int index)
            => r.GetInt64(index);

        public static string getString(this NpgsqlDataReader r, int index)
            => r.GetString(index);

        public static string? tryGetString(this NpgsqlDataReader r, int index) {
            return r.IsDBNull(index) ? null : r.GetString(index);
        }

        public static bool getBool(this NpgsqlDataReader r, int index)
            => r.GetBoolean(index);

        public static byte[] getBytes(this NpgsqlDataReader r, int index)
            => r.GetFieldValue<byte[]>(index);

        public static DateTime getDateTime(this NpgsqlDataReader r, int index)
            => r.GetDateTime(index);

    }


    public class DAOUtils {

        public static async Task CreateTable(string table) {

            await using var conn = new NpgsqlConnection(DAOManager.connection_string);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(table, conn);
            await cmd.ExecuteNonQueryAsync();
        }

        public static async Task<TResult> Query<TResult>(string sql, Func<NpgsqlCommand, Task<TResult>> func) {

            await using var conn = new NpgsqlConnection(DAOManager.connection_string);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);
            return await func(cmd);

        }

        public static async Task<long> Count(string collection) {

            string sql = $"SELECT COUNT(*) FROM {collection};";
            return await DAOUtils.Query(sql, async cmd =>
                Convert.ToInt64(await cmd.ExecuteScalarAsync()));

        }

        private static void _addParametersList(NpgsqlCommand cmd, Query? query) {

            if (query == null) 
                return;

            foreach (var (key, value) in query.getParameters())
                cmd.Parameters.AddWithValue(key, value ?? DBNull.Value);

        }

        public static async Task<DAOListing<T>> List<T>(string collection, string attributes, Query? query, Func<NpgsqlDataReader, T> serializer) {

            var where = query?.getFilter() ?? "";
            var order = query?.getSort() ?? "";
            var limit = query?.limit is not null ? "LIMIT @limit" : "";
            var offset = query?.offset is not null ? "OFFSET @offset" : "";

            var sql_list = $"SELECT {attributes} FROM {collection} {where} {order} {limit} {offset};";
            var sql_count = $"SELECT COUNT(*) FROM {collection} {where};";

            await using var conn = new NpgsqlConnection(DAOManager.connection_string);
            await conn.OpenAsync();

            long total_elements;
            var list = new List<T>();

            // Counting
            await using (var cmd = new NpgsqlCommand(sql_count, conn)) {
                _addParametersList(cmd, query);
                total_elements = (long) (await cmd.ExecuteScalarAsync() ?? 0);
            }

            // Listing
            await using (var cmd = new NpgsqlCommand(sql_list, conn)) {

                _addParametersList(cmd, query);

                if (query?.limit != null)
                    cmd.Parameters.AddWithValue("@limit", query.limit);

                if (query?.offset != null)
                    cmd.Parameters.AddWithValue("@offset", query.offset);

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    list.Add(serializer(reader));

            }

            return new DAOListing<T>(total_elements, list);

        }

    }

}