namespace PacketTemplates {

    public abstract class TemplateValidatorField {
        public bool is_required { get; set; }
        public bool is_list { get; set; }
    }

    public class TemplateValidatorItem : TemplateValidatorField {

        public Type datatype { get; set; }

        public TemplateValidatorItem(bool is_required, Type datatype, bool is_list) {
            this.is_required = is_required;
            this.datatype = datatype;
            this.is_list = is_list;
        }
    }

    public class TemplateValidatorObject : TemplateValidatorField {

        public Dictionary<string, TemplateValidatorField> obj { get; set; }

        public TemplateValidatorObject(bool is_required, bool is_list) {
            this.is_required = is_required;
            this.is_list = is_list;
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

    public class TemplateValidatorQuery {
        
        public bool has_page { get; set; }
        public HashSet<string> sort_opts {get; set;}
        public Dictionary<string, TemplateValidatorQueryItem> queries { get; set; }

        public TemplateValidatorQuery(bool has_page) {
            this.has_page = has_page;
            this.queries = new();
            this.sort_opts = new();

            if (has_page) {
                this.queries["page"] = new TemplateValidatorQueryItem(typeof(long));
                this.queries["limit"] = new TemplateValidatorQueryItem(typeof(long));
                this.queries["sort"] = new TemplateValidatorQueryItem(typeof(string));
            }

        }

        public void add_queries(Dictionary<string, TemplateValidatorQueryItem> queries) {
            
            foreach(string key in queries.Keys)
                this.queries[key] = queries[key];

        }

        public void add_sort_opts(HashSet<string> sort_opts) {
            this.sort_opts = sort_opts.Select(s => s.ToLower()).ToHashSet();
        }

    }

}
