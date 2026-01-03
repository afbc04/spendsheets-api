using System.Text.RegularExpressions;
using PacketTemplates;
using Queries;

namespace PacketHandlers {

    public class PacketQueryValidatorPage {

        public long page;
        public long limit;
        public IList<QueryOrderItem> sort {get; private set;}

        public PacketQueryValidatorPage(long page, long limit, IList<QueryOrderItem> sort) {

            this.page = page;
            this.limit = limit;
            this.sort = sort;

        }

    }

    public class PacketQueryValidatorObject {

        public List<string> unnecessary_fields;
        public Dictionary<string, string> wrong_datatype_fields;
        public List<string> wrong_page_format;

        public PacketQueryValidatorObject() {
            this.unnecessary_fields = new();
            this.wrong_datatype_fields = new();
            this.wrong_page_format = new();
        }

        public PacketQueryValidatorObject(List<string> unnecessary_fields, Dictionary<string,string> wrong_datatype_fields, List<string> wrong_page_format) {
            this.unnecessary_fields = unnecessary_fields;
            this.wrong_datatype_fields = wrong_datatype_fields;
            this.wrong_page_format = wrong_page_format;
        }

    }

    public static class PacketQueryValidatorFunctions {

        public static (PacketQueryValidatorObject,QueriesRequest?) validate_packet_query_fields(Dictionary<string, object> data, TemplateValidatorQuery requirements) {
            
            var pqv = new PacketQueryValidatorObject();

            foreach (var kv in data) {

                var key = kv.Key;
                var value = kv.Value;

                if (requirements.queries.ContainsKey(key) == false) {
                    pqv.unnecessary_fields.Add(key);
                    continue;
                }

                var key_validator = requirements.queries[key];
                pqv = _validate_packet_body_fields_values(key,value, key_validator, pqv);
                
            }

            PacketQueryValidatorPage? page = null;

            if (pqv.wrong_datatype_fields.Count() == 0) {
                (page, List<string> page_error_format) = validate_packet_query_fields_page(data,requirements.has_page,requirements.sort_opts);
                pqv.wrong_page_format = page_error_format;
            }

            var queriable = page == null ? new QueriesRequest(null,null,[],data!) : new QueriesRequest(page.page,page.limit,page.sort,data!);
            return (pqv,queriable);

        }

        private static PacketQueryValidatorObject _validate_packet_body_fields_values(string key, object item, TemplateValidatorQueryItem requirements, PacketQueryValidatorObject pqv) {

            if (requirements.datatype == typeof(double)) {

                if (Regex.IsMatch((string) item,@"^-?\d+\.\d+$") == false)
                    pqv.wrong_datatype_fields[key] = PacketUtils.getType(requirements.datatype);

            }
            else if (requirements.datatype == typeof(long)) {

                if (Regex.IsMatch((string) item,@"^-?\d+$") == false)
                    pqv.wrong_datatype_fields[key] = PacketUtils.getType(requirements.datatype);

            }
            else if (requirements.datatype == typeof(bool)) {

                if (Regex.IsMatch((string) item,@"^(true|false)$") == false)
                    pqv.wrong_datatype_fields[key] = PacketUtils.getType(requirements.datatype);

            }
            else if (requirements.datatype == typeof(string)) {

                if (Regex.IsMatch((string) item,@"^[\w%_\d{} :\.\-,]+$") == false)
                    pqv.wrong_datatype_fields[key] = PacketUtils.getType(requirements.datatype);

            }
            else {
                pqv.wrong_datatype_fields[key] = PacketUtils.getType(requirements.datatype);
            }
            
            return pqv;

        }

        private static (PacketQueryValidatorPage?,List<string>) validate_packet_query_fields_page(Dictionary<string, object> data, bool has_pageable, HashSet<string> sort_opts) {
            
            var error_list = new List<string>();

            if (has_pageable == false)
                return (null,error_list);

            var page = Convert.ToInt64(PacketUtils.get_value(data,"page") ?? PageRules.page_default);
            var limit = Convert.ToInt64(PacketUtils.get_value(data,"limit") ?? PageRules.limit_default);
            var sort = (string?) PacketUtils.get_value(data,"sort");

            if (page <= 0)
                error_list.Add("Page number must be a positive number");

            if (limit < PageRules.limit_min) {
                error_list.Add($"Limit is too low. Minimum is {PageRules.limit_min}");
            }

            if (limit > PageRules.limit_max) {
                error_list.Add($"Limit is too high. Maximum is {PageRules.limit_max}");
            }

            List<QueryOrderItem> sort_list = new();

            if (sort != null) {

                if (Regex.IsMatch(sort,@"{(\w[\w_]*:-?1)(,(\w[\w_]*:-?1))*}") == false)
                    error_list.Add($"Sort arguments are not valid. List should have the format : {{<name>:(1 or -1)}} (item divided by ',')");
                else {

                    string[] sorting_tokens = sort.Replace("{","").Replace("}","").Split(",");
                    string sort_options = string.Join(", ",sort_opts);

                    foreach (string token in sorting_tokens) {

                        string[] token_args = token.Split(":");
                        bool is_asc = Convert.ToInt32(token_args[1]) == 1;

                        if (sort_opts.Contains(token_args[0].ToLower()) == false) 
                            error_list.Add($"Sort argument {token_args[0]} is not valid. Try these ones : [{sort_options}]");
                        else 
                            sort_list.Add(new QueryOrderItem(token_args[0],is_asc));

                    }

                }

            }

            if (error_list.Count() > 0)
                return (null,error_list);
            else {
                PacketQueryValidatorPage page_obj = new PacketQueryValidatorPage((long) page!,(long) limit!,sort_list);
                return (page_obj,error_list);
            }

        }

    }
}
