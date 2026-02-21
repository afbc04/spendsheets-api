namespace Templates {

    public static class TokenTemplate {

        public static TemplatePacket ValidateToken() => TokenTemplate.validate;
        public static TemplatePacket ObtainToken() => TokenTemplate.obtain_token;
        public static TemplatePacket SetReaderToken() => TokenTemplate.create_reader_token;
        public static TemplatePacket InvalidateToken() => TokenTemplate.invalidate_token;



        private static readonly TemplatePacket validate = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Non()
        );

        private static readonly TemplatePacket obtain_token = new(
            TemplateAuth.NotRequired(),
            TemplateQuery.Non(),
            TemplateBody.Required(new() {
                ["secret"] = TemplateItem.RequiredNotNull(typeof(string)),
                ["writer"] = TemplateItem.RequiredNotNull(typeof(bool))
            })
        );

        private static readonly TemplatePacket create_reader_token = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Required(new() {
                ["secretReader"] = TemplateItem.RequiredNotNull(typeof(string)),
                ["expiresIn"] = TemplateItem.NotRequiredNotNull(typeof(long))
            })
        );

        private static readonly TemplatePacket invalidate_token = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Required(new() {
                ["writer"] = TemplateItem.RequiredNotNull(typeof(bool))
            })
        );

    }

}