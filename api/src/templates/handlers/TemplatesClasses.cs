namespace Templates {

    public class TemplatePacket {

        public TemplateAuth? auth {get; set;}
        public TemplateQuery? queries {get; set;}
        public TemplateBody? body {get; set;}

        public TemplatePacket(TemplateAuth? auth, TemplateQuery? queries, TemplateBody? body) {
            this.auth = auth;
            this.body = body;
            this.queries = queries;
        }

    }

}
