namespace Templates {

    public class TemplateQueryItem {

        public Type datatype { get; set; }

        public TemplateQueryItem(Type datatype) {
            this.datatype = datatype;
        }

        public static TemplateQueryItem Item(Type datatype) =>
            new TemplateQueryItem(datatype);

    }

    public struct TemplateQuerySortItem {

        public bool is_case_insensitive { get; set; }
        public bool is_hidden { get; set; }

        public TemplateQuerySortItem(bool is_case_insensitive, bool is_hidden) {
            this.is_case_insensitive = is_case_insensitive;
            this.is_hidden = is_hidden;
        }

        public static TemplateQuerySortItem Default() =>
            new TemplateQuerySortItem(false,false);

        public static TemplateQuerySortItem CaseInsensitive() =>
            new TemplateQuerySortItem(true,false);

        public static TemplateQuerySortItem HiddenDefault() =>
            new TemplateQuerySortItem(false,true);

        public static TemplateQuerySortItem HiddenCaseInsensitive() =>
            new TemplateQuerySortItem(true,true);

    }

    public class TemplateQuery {
        
        public bool has_page { get; set; }
        public Dictionary<string, TemplateQuerySortItem>? sort_opts {get; set;}
        public Dictionary<string, TemplateQueryItem> queries { get; set; }

        public TemplateQuery(bool has_page, Dictionary<string, TemplateQueryItem>? queries, Dictionary<string, TemplateQuerySortItem>? sort_opts) {
            this.has_page = has_page;
            this.queries = new();
            this.sort_opts = null;

            if (has_page) {
                this.queries["page"] = new TemplateQueryItem(typeof(long));
                this.queries["limit"] = new TemplateQueryItem(typeof(long));
            }

            if (queries != null)
                foreach(string key in queries.Keys)
                    this.queries[key] = queries[key];

            if (sort_opts != null) {

                this.sort_opts = new();
                this.queries["sort"] = new TemplateQueryItem(typeof(string));

                foreach(string key in sort_opts.Keys)
                    this.sort_opts[key] = sort_opts[key];

            }

        }

        public static TemplateQuery OnlyPage() =>
            new TemplateQuery(true,null,null);

        public static TemplateQuery PageAndSorts(Dictionary<string, TemplateQuerySortItem> sorts) =>
            new TemplateQuery(true,null,sorts);

        public static TemplateQuery PageAndSortsAndQueries(Dictionary<string, TemplateQuerySortItem> sorts, Dictionary<string, TemplateQueryItem> queries) =>
            new TemplateQuery(true,queries,sorts);

        public static TemplateQuery PageAndQueries(Dictionary<string, TemplateQueryItem> queries) =>
            new TemplateQuery(true,queries,null);

        public static TemplateQuery OnlyQueries(Dictionary<string, TemplateQueryItem> queries) =>
            new TemplateQuery(false,queries,null);

        public static TemplateQuery? Non() =>
            null;

    }

}