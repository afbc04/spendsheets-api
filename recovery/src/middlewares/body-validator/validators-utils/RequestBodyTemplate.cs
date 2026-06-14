
    public class RequestBodyTemplate {
        
        public bool IsRequired { get; set; }
        public Dictionary<string, ValidatorBodyFieldMiddleware> body { get; set; }

        public RequestBodyTemplate(bool is_required, Dictionary<string, ValidatorBodyFieldMiddleware> body) {
            this.IsRequired = is_required;
            this.body = body;
        }

        public static RequestBodyTemplate Required(Dictionary<string, ValidatorBodyFieldMiddleware> body) =>
            new RequestBodyTemplate(true,body);

        public static RequestBodyTemplate NotRequired(Dictionary<string, ValidatorBodyFieldMiddleware> body) =>
            new RequestBodyTemplate(false,body);

        public static RequestBodyTemplate? Non() =>
            null;

    }
