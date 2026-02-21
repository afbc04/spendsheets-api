using System.Text.Json;
using Templates;
using System.Globalization;

namespace PacketHandlers {

    public class PacketBodyValidatorObject {

        public List<string> missing_required_fields;
        public List<string> unnecessary_fields;
        public Dictionary<string, string> wrong_datatype_fields;

        public PacketBodyValidatorObject() {
            this.missing_required_fields = new();
            this.unnecessary_fields = new();
            this.wrong_datatype_fields = new();
        }

        public PacketBodyValidatorObject(List<string> missing_required_fields, List<string> unnecessary_fields, Dictionary<string,string> wrong_datatype_fields) {
            this.missing_required_fields = missing_required_fields;
            this.unnecessary_fields = unnecessary_fields;
            this.wrong_datatype_fields = wrong_datatype_fields;
        }

        public void Merge(PacketBodyValidatorObject other) {

            this.missing_required_fields.Concat(other.missing_required_fields);
            this.unnecessary_fields.AddRange(other.unnecessary_fields);

            foreach (var kv in other.wrong_datatype_fields)
                this.wrong_datatype_fields[kv.Key] = kv.Value;
        }

    }

    public static class PacketBodyValidatorFunctions {

        public static PacketBodyValidatorObject ValidateBodyFieldsRecursive(Dictionary<string, object> data, Dictionary<string, TemplateField> requirements, string path) {
            
            var pbv = new PacketBodyValidatorObject();

            pbv.missing_required_fields = requirements
                .Where(kv => kv.Value.is_required)
                .Select(kv => $"{path}{kv.Key}")
                .ToList();

            foreach (var kv in data) {

                var key = kv.Key;
                var value = kv.Value;
                var key_with_path = $"{path}{key}";

                if (requirements.ContainsKey(key) == false) {
                    pbv.unnecessary_fields.Add(key_with_path);
                    continue;
                }

                var key_validator = requirements[key];

                if (key_validator.is_required && pbv.missing_required_fields.Contains(key_with_path))
                    pbv.missing_required_fields.Remove(key_with_path);

                var values_to_analyse = value is IEnumerable<object> list ? list.ToList() : new List<object> { value };

                if (key_validator.is_list && value is not IEnumerable<object>)
                    pbv.wrong_datatype_fields[key_with_path] = "list";
                
                else
                    pbv = _validate_packet_body_fields_rec_values(values_to_analyse, key_validator, key_with_path, pbv);
                
            }

            return pbv;
        }

        private static PacketBodyValidatorObject _validate_packet_body_fields_rec_values(List<object> values, TemplateField requirements, string path, PacketBodyValidatorObject pbv) {
            
            int listLen = values.Count;

            for (int i = 0; i < values.Count; i++) {

                var item = values[i];
                var path_in_case_of_error = path;

                if (requirements.is_list)
                    path_in_case_of_error += $"[{i}]";

                if (requirements is TemplateObject)
                    pbv = _validate_packet_body_fields_rec_value_object(item, (TemplateObject) requirements, path_in_case_of_error, pbv);
                else
                    pbv = _validate_packet_body_fields_rec_value_item(item, (TemplateItem) requirements, path_in_case_of_error, pbv);
            }

            return pbv;
        }

        private static PacketBodyValidatorObject _validate_packet_body_fields_rec_value_item(object item, TemplateItem requirements, string path, PacketBodyValidatorObject pbv) {
            
            if (item == null) {
                if (!requirements.allow_null)
                    pbv.wrong_datatype_fields[path] =
                        PacketUtils.getType(requirements.datatype);
                return pbv;
            }

            if (requirements.datatype == typeof(DateOnly)) {

                if (item is not string s ||
                    !DateOnly.TryParseExact(
                        s,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out _))
                {
                    pbv.wrong_datatype_fields[path] =
                        "date (yyyy-MM-dd)" +
                        (requirements.allow_null ? " | null" : "");
                }

                return pbv;
            }

            if (requirements.datatype != item.GetType()) {
                pbv.wrong_datatype_fields[path] =
                    PacketUtils.getType(requirements.datatype) +
                    (requirements.allow_null ? " | null" : "");
            }

            return pbv;
        }


        private static PacketBodyValidatorObject _validate_packet_body_fields_rec_value_object(object obj, TemplateObject requirements, string path, PacketBodyValidatorObject pbv) {
            
            if (obj != null) {

                if (obj is not Dictionary<string, object>)
                    pbv.wrong_datatype_fields[path] = string.Concat("object",requirements.allow_null ? " | null" : "");
                
                else {
                    var pbv_from_object = ValidateBodyFieldsRecursive((Dictionary<string, object>) obj, requirements.obj, $"{path}.");
                    pbv.Merge(pbv_from_object);
                }

            }
            else if (obj == null & requirements.allow_null == false)
                pbv.wrong_datatype_fields[path] = "object";

            return pbv;
        }   

        public static Dictionary<string, object> convert_json_to_dict(JsonElement element) {

            var dict = new Dictionary<string, object>();

            foreach (var item in element.EnumerateObject())
                dict[item.Name] = _convert_json_item(item.Value);

            return dict;

        }

        private static object _convert_json_item(JsonElement element) {
            
            switch (element.ValueKind) {

                case JsonValueKind.Object:
                    return convert_json_to_dict(element);

                case JsonValueKind.Array:
                    return element.EnumerateArray()
                        .Select(_convert_json_item)
                        .ToList();

                case JsonValueKind.String:
                    return element.GetString()!;

                case JsonValueKind.Number:
                    if (element.TryGetInt64(out long l))
                        return l;
                    else if (element.TryGetDouble(out double d))
                        return d;
                    return element.GetRawText();

                case JsonValueKind.True:
                    return true;

                case JsonValueKind.False:
                    return false;

                case JsonValueKind.Null:
                    return null!;

                default:
                    return element.GetRawText();

            }

        }

    }
}
