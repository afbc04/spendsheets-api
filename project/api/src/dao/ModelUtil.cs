using Npgsql;
using Queries;

namespace Models {

    public class ModelUtil {

        public static async Task<TResult> execute_query<TResult>(string sql, Func<NpgsqlCommand, Task<TResult>> func) {

            await using var conn = new NpgsqlConnection(ModelsManager.connection_string);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);
            return await func(cmd);

        }

        public static async Task<long> execute_get_size(string collection) {

            string sql = $"SELECT COUNT(*) FROM {collection};";
            return await ModelUtil.execute_query(sql, async cmd =>
                Convert.ToInt64(await cmd.ExecuteScalarAsync()));

        }

        public static async Task<ModelListing<T>> execute_get_list<T>(IQuery querie, string collection, string attributes, Func<NpgsqlDataReader, T> serializer) {

            string filtering = querie.get_sql_filtering();

            string listing_sql = $"SELECT {attributes} FROM {collection} {filtering} {querie.get_sql_listing()};";
            string count_sql = $"SELECT COUNT(*) FROM {collection} {filtering};";

            Console.WriteLine($"FILTERING {listing_sql}");

            await using var conn = new NpgsqlConnection(ModelsManager.connection_string);
            await conn.OpenAsync();

            await using var transaction = await conn.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);

            await using (var cmd_trans = conn.CreateCommand()) {
                cmd_trans.Transaction = transaction;
                cmd_trans.CommandText = "SET TRANSACTION READ ONLY;";
                await cmd_trans.ExecuteNonQueryAsync();
            }

            long total = 0;
            await using (var cmd_count = conn.CreateCommand()) {
                cmd_count.Transaction = transaction;
                cmd_count.CommandText = count_sql;
                total = (long)(await cmd_count.ExecuteScalarAsync() ?? 0);
            }

            var list = new List<T>();
            await using (var cmd_list = conn.CreateCommand()) {

                cmd_list.Transaction = transaction;
                cmd_list.CommandText = listing_sql;

                await using var reader = await cmd_list.ExecuteReaderAsync();
                while (await reader.ReadAsync()) {
                    list.Add(serializer(reader));
                }
            }

            await transaction.CommitAsync();
            return new ModelListing<T>(total, list);

        }

        public static string? get_string(NpgsqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? null : reader.GetString(index);
        }

        public static int? get_int(NpgsqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? null : reader.GetInt32(index);
        }

        public static double? get_double(NpgsqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? null : reader.GetDouble(index);
        }

        public static bool? get_bool(NpgsqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? null : reader.GetBoolean(index);
        }

        public static DateOnly? get_date(NpgsqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? null : DateOnly.FromDateTime(reader.GetDateTime(index));
        }

        public static DateTime? get_date_time(NpgsqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? null : reader.GetDateTime(index);
        }

        public static ulong get_money(NpgsqlDataReader reader, int index) {
            return (ulong) reader.GetInt64(index);
        }

    }

}