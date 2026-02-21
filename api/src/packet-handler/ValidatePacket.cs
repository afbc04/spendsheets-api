using System.Text.Json;
using System.Text.RegularExpressions;
using Templates;
using Controller;
using Queries;

namespace PacketHandlers {

    public static class PacketValidator {

        private static readonly string auth_type = "Bearer";

        public static string? validate_packet_auth(HttpRequest request, TemplateAuth? auth) {

            if (auth != null && auth.is_required == true) {

                if (request.Headers.TryGetValue("Authorization", out var auth_header_values) == false)
                    throw new ValidatePacketException(401,"This endpoint requires a token. Please provide it");

                string auth_header = auth_header_values.ToString();

                if (auth_header.StartsWith($"{PacketValidator.auth_type} ") == false)
                    throw new ValidatePacketException(401,$"This endpoint requires a bearer token: `Authorization: {PacketValidator.auth_type} <token>`");

                if (Regex.IsMatch(auth_header, $@"{PacketValidator.auth_type} .+") == false || Regex.IsMatch(auth_header, $@"{PacketValidator.auth_type} null"))
                    throw new ValidatePacketException(401,"Token not provided");

                return auth_header.Substring($"{PacketValidator.auth_type} ".Length);
            
            }
            else if (auth != null && auth.is_required == false) {

                if (request.Headers.TryGetValue("Authorization", out var auth_header_values) == false)
                    return null;

                string auth_header = auth_header_values.ToString();

                if (auth_header.StartsWith($"{PacketValidator.auth_type} ") == false)
                    throw new ValidatePacketException(401,$"This endpoint only accpets bearer tokens: `Authorization: {PacketValidator.auth_type} <token>`");

                if (Regex.IsMatch(auth_header, $@"{PacketValidator.auth_type} .+") == false || Regex.IsMatch(auth_header, $@"{PacketValidator.auth_type} null"))
                    throw new ValidatePacketException(401,"Token not provided");

                return auth_header.Substring($"{PacketValidator.auth_type} ".Length);
            
            }

            else if (auth == null && request.Headers.ContainsKey("Authorization"))
                throw new ValidatePacketException(417,"This endpoint doesn't want token. Do not provide it");

            return null;

        }

        public static async Task<IDictionary<string, object>> validate_packet_body(HttpRequest request, TemplateBody? body) {

            Dictionary<string, object> data = new();
            string request_body;

            using (var reader = new StreamReader(request.Body)) {
                request_body = await reader.ReadToEndAsync();
            }

            bool is_body_empty = string.IsNullOrWhiteSpace(request_body);

            if (body == null) {

                if (is_body_empty == false)
                    throw new ValidatePacketException(417, "This endpoint doesn't accept body. Do not send it");

            }
            else {

                if (body.is_required && is_body_empty)
                    throw new ValidatePacketException(417, "This endpoint requires body. Please provide it");

                if (is_body_empty == false) {

                    data = _validate_packet_is_json(request_body);
                    _validate_packet_body_fields(data, body.body);

                }
            }

            return data;

        }

        private static Dictionary<string, object> _validate_packet_is_json(string body) {

            try {

                using var doc = JsonDocument.Parse(body);
                var json = doc.RootElement;

                if (json.ValueKind != JsonValueKind.Object)
                   throw new ValidatePacketException(417, "Body JSON must be an object");

                return PacketBodyValidatorFunctions.convert_json_to_dict(json);

            }
            catch {
                throw new ValidatePacketException(417, "Body is not a valid JSON");
            }
        }

        private static void _validate_packet_body_fields(Dictionary<string, object> data, Dictionary<string, TemplateField> requirements) {

            var pbv = PacketBodyValidatorFunctions.ValidateBodyFieldsRecursive(data, requirements, "");

            if (pbv.missing_required_fields.Count > 0)
                throw new ValidatePacketException(417, "Some required values are missing. Please provide them", new Dictionary<string,object> { { "missing_values", pbv.missing_required_fields } });

            if (pbv.wrong_datatype_fields.Count > 0)
                throw new ValidatePacketException(417, "There are some values with wrong datatype. Please correct them", new Dictionary<string,object> { { "wrong_datatypes", pbv.wrong_datatype_fields } });

            if (pbv.unnecessary_fields.Count > 0)
                throw new ValidatePacketException(417, "There are some extra values. Please remove them", new Dictionary<string,object> { { "extra_values", pbv.unnecessary_fields } });

        }

        public static QueriesRequest validate_packet_queries(HttpRequest request, TemplateQuery? queries) {

            var data = request.Query.ToDictionary(
                kv => kv.Key,
                kv => (object) kv.Value.ToString()
            );

            if (queries == null) {

                if (data.Count > 0)
                    throw new ValidatePacketException(417, "This endpoint doesn't accept query parameters. Do not send them.");

                return new QueriesRequest(null,null,[],new Dictionary<string,object?>());

            }

            return _validate_packet_queries_fields(data, queries!);

        }

        private static QueriesRequest _validate_packet_queries_fields(Dictionary<string, object> data, TemplateQuery requirements) {

            (var pqv, var queries_final) = PacketQueryValidatorFunctions.validate_packet_query_fields(data, requirements);

            if (pqv.wrong_datatype_fields.Count > 0)
                throw new ValidatePacketException(417, "There are some values with wrong datatype. Please correct them", new Dictionary<string,object> { { "wrong_datatypes", pqv.wrong_datatype_fields } });

            if (pqv.unnecessary_fields.Count > 0)
                throw new ValidatePacketException(417, "There are some extra query parameters. Please remove them", new Dictionary<string,object> { { "extra_values", pqv.unnecessary_fields } });

            if (pqv.wrong_page_format.Count > 0)
                throw new ValidatePacketException(417, "There are some page parameters with wrong format. Please correct them", new Dictionary<string,object> { { "page_errors", pqv.wrong_page_format } });

            return queries_final!;

        }

        public static async Task<(bool, PacketFail?, PacketExtracted?)> validate_packet(HttpRequest request, TemplatePacket template) {

            try {

                IDictionary<string,object>? extracted_body = null;
                QueriesRequest? extracted_queries = null;

                string? token = validate_packet_auth(request, template.auth);
                extracted_body = await validate_packet_body(request, template.body);
                extracted_queries = validate_packet_queries(request, template.queries);

                return (true,null,new PacketExtracted(token,extracted_body,extracted_queries));

            }
            catch (ValidatePacketException ex) {
                return (false,ex.packet_failure,null);
            }

        }

    }

    public class ValidatePacketException : Exception {

        public PacketFail packet_failure;

        public ValidatePacketException(PacketFail packet) {
            this.packet_failure = packet;
        }

        public ValidatePacketException(int status_code, string error_message, IDictionary<string,object>? extra_message = null) {
            this.packet_failure = new PacketFail(status_code,error_message,extra_message);
        }
    }
    
}






