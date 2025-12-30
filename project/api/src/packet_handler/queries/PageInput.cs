namespace Pages {

    public class PageInput {

        public long page { get; private set; }
        public long limit { get; private set; }
        public string? sort { get; private set; }

        public PageInput(long page, long limit, List<(string, bool)> sort_args) {

            this.page = page;
            this.limit = limit;
            this.sort = null;

            if (sort_args.Count() > 0) {

                string sorting_sql = "ORDER BY ";
                List<string> sorting_args = new();

                foreach ((string arg, bool is_asc) in sort_args)
                {
                    string asc = is_asc == true ? "ASC" : "DESC";
                    sorting_args.Add($"{arg} {asc}");
                }

                sorting_sql += string.Join(", ", sorting_args);
                this.sort = sorting_sql;

            }

        }

        public string get_sql_listing() {

            string sql = "";

            if (this.sort != null)
                sql += $" {this.sort}";

            sql += $" LIMIT {this.limit} OFFSET {(this.page - 1) * this.limit}";

            return sql;

        }

    }

}

