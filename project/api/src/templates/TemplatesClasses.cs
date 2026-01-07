namespace PacketTemplates {

    public abstract class TemplateValidatorField {
        public bool is_required { get; set; }
        public bool is_list { get; set; }
        public bool allow_null { get; set; }
    }

    public class TemplateValidatorItem : TemplateValidatorField {

        public Type datatype { get; set; }

        public TemplateValidatorItem(bool is_required, Type datatype, bool is_list, bool allow_null) {
            this.is_required = is_required;
            this.datatype = datatype;
            this.is_list = is_list;
            this.allow_null = allow_null;
        }
    }

    public class TemplateValidatorObject : TemplateValidatorField {

        public Dictionary<string, TemplateValidatorField> obj { get; set; }

        public TemplateValidatorObject(bool is_required, bool is_list, bool allow_null) {
            this.is_required = is_required;
            this.is_list = is_list;
            this.allow_null = allow_null;
            this.obj = new();
        }

        public void add_child(Dictionary<string, TemplateValidatorField> obj) {
            this.obj = obj;
        }
    }

    public class TemplateValidatorBody {
        
        public bool is_required { get; set; }
        public Dictionary<string, TemplateValidatorField> body { get; set; }

        public TemplateValidatorBody(bool is_required) {
            this.is_required = is_required;
            this.body = new();
        }

        public void add_body(Dictionary<string, TemplateValidatorField> body) {
            this.body = body;
        }
    }

    public class TemplateValidatorAuth {
        
        public bool is_required { get; set; }

        public TemplateValidatorAuth(bool is_required) {
            this.is_required = is_required;
        }

    }

    public class TemplateValidatorQueryItem {

        public Type datatype { get; set; }

        public TemplateValidatorQueryItem(Type datatype) {
            this.datatype = datatype;
        }

    }

    public struct TemplateValidatorQuerySortItem {

        public bool is_case_insensitive { get; set; }

        public TemplateValidatorQuerySortItem(bool is_case_insensitive) {
            this.is_case_insensitive = is_case_insensitive;
        }

    }

    public class TemplateValidatorQuery {
        
        public bool has_page { get; set; }
        public Dictionary<string, TemplateValidatorQuerySortItem>? sort_opts {get; set;}
        public Dictionary<string, TemplateValidatorQueryItem> queries { get; set; }

        public TemplateValidatorQuery(bool has_page) {
            this.has_page = has_page;
            this.queries = new();
            this.sort_opts = null;

            if (has_page) {
                this.queries["page"] = new TemplateValidatorQueryItem(typeof(long));
                this.queries["limit"] = new TemplateValidatorQueryItem(typeof(long));
            }

        }

        public void add_queries(Dictionary<string, TemplateValidatorQueryItem> queries) {
            
            foreach(string key in queries.Keys)
                this.queries[key] = queries[key];

        }

        public void add_sort_opts(Dictionary<string, TemplateValidatorQuerySortItem>? sort_opts) {
            
            if (sort_opts != null) {

                this.sort_opts = new();
                this.queries["sort"] = new TemplateValidatorQueryItem(typeof(string));

                foreach(string key in sort_opts.Keys)
                    this.sort_opts[key] = sort_opts[key];

            }
            else
                this.sort_opts = null;

        }

    }

}
