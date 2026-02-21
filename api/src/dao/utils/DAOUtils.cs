using Npgsql;

namespace DAO {

    public class DAOUtils {

        public static async Task ExecQuery(string query) {

            await using var conn = new NpgsqlConnection(DAOManager.connection_string);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(query, conn);
            await cmd.ExecuteNonQueryAsync();

        }

        public static async Task<TResult> Query<TResult>(string sql, Func<NpgsqlCommand, Task<TResult>> func) {

            await using var conn = new NpgsqlConnection(DAOManager.connection_string);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);
            return await func(cmd);

        }

        private static void _addParametersList(NpgsqlCommand cmd, Query? query) {

            if (query == null) 
                return;

            foreach (var (key, value) in query.getParameters())
                cmd.Parameters.AddWithValue(key, value ?? DBNull.Value);

        }

        public static async Task<DAOListing<T>> List<T>(
            string collection, 
            string attributes, 
            Query? query, 
            Func<NpgsqlDataReader, T> serializer
        ) {

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
                total_elements = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
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