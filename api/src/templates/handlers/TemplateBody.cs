namespace Templates {

    public class TemplateBody {
        
        public bool is_required { get; set; }
        public Dictionary<string, TemplateField> body { get; set; }

        public TemplateBody(bool is_required, Dictionary<string, TemplateField> body) {
            this.is_required = is_required;
            this.body = body;
        }

        public static TemplateBody Required(Dictionary<string, TemplateField> body) =>
            new TemplateBody(true,body);

        public static TemplateBody NotRequired(Dictionary<string, TemplateField> body) =>
            new TemplateBody(false,body);

        public static TemplateBody? Non() =>
            null;

    }

}