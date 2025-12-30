using Npgsql;
using Queries;

namespace Models {
/*
    public class CountryCodeModel {

        private static CountryCode _serialize(NpgsqlDataReader reader) {
            return new CountryCode(reader.GetString(0), reader.GetString(1));
        }

        public async Task<CountryCode?> get(string ID) {

            string sql = "SELECT id, name FROM CountryCodes WHERE id = @id;";
            return await ModelUtil.execute_query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id",ID);
                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                    return _serialize(reader);

                return null;

            });

        }

        public async Task<bool> contains(string ID) {

            string sql = "SELECT COUNT(*) FROM CountryCodes WHERE id = @id;";
            return await ModelUtil.execute_query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                var count = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                return count > 0;

            });

        }

        public async Task<bool> delete(string ID) {

            string sql = "DELETE FROM CountryCodes WHERE id = @id";
            return await ModelUtil.execute_query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", ID);
                var lines = await cmd.ExecuteNonQueryAsync();
                return lines > 0;

            });

        }

        public async Task<bool> insert(CountryCode cc) {

            try {

                string sql = "INSERT INTO CountryCodes (id, name) VALUES (@id, @name);";

                return await ModelUtil.execute_query(sql, async cmd => {
                    
                    cmd.Parameters.AddWithValue("@id", cc.ID);
                    cmd.Parameters.AddWithValue("@name", cc.name);
                    
                    await cmd.ExecuteNonQueryAsync();
                    return true;

                });

            }
            catch {
                return false;
            }

        }

        public async Task<bool> update(CountryCode cc) {

            string sql = "UPDATE CountryCodes SET name = @name WHERE id = @id;";

            return await ModelUtil.execute_query(sql, async cmd => {

                cmd.Parameters.AddWithValue("@id", cc.ID);
                cmd.Parameters.AddWithValue("@name", cc.name);

                var lines = await cmd.ExecuteNonQueryAsync();
                return lines > 0;

            });

        }

        public async Task<ModelListing<CountryCode>> values(QueryCountryCode querie) {
            return await ModelUtil.execute_get_list(querie,"CountryCodes","id, name",_serialize);
        }

        public async Task<IList<string>> keys() {

            string sql = "SELECT id FROM CountryCodes;";
            return await ModelUtil.execute_query(sql, async cmd => {

                var list = new List<string>();
                await using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                    list.Add(reader.GetString(0));
                
                return list;

            });

        }

        public async Task<long> size() {
            return await ModelUtil.execute_get_size("CountryCodes");
        }

        public async Task<bool> empty() {
            return await this.size() == 0;
        }

    }*/
}
