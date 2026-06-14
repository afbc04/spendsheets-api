namespace Templates {

    public class TemplateAuth {
        
        public bool is_required { get; set; }

        public TemplateAuth(bool is_required) {
            this.is_required = is_required;
        }

        public static TemplateAuth Required() =>
            new TemplateAuth(true);

        public static TemplateAuth NotRequired() =>
            new TemplateAuth(false);

        public static TemplateAuth? Non() =>
            null;

    }

}