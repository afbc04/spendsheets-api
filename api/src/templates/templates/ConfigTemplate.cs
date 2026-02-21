namespace Templates {

    public static class ConfigTemplate {

        public static TemplatePacket Get() => ConfigTemplate.get;
        public static TemplatePacket Patch() => ConfigTemplate.patch;
        public static TemplatePacket Put() => ConfigTemplate.put;



        public static readonly TemplatePacket get = new(
            TemplateAuth.NotRequired(),
            TemplateQuery.Non(),
            TemplateBody.Non()
        );

        public static readonly TemplatePacket patch = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Required(new() {
                ["name"] = TemplateItem.NotRequiredNull(typeof(string)),
                ["public"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                ["initialMoney"] = TemplateItem.NotRequiredNotNull(typeof(double)),
                ["lostMoney"] = TemplateItem.NotRequiredNotNull(typeof(double)),
                ["savedMoney"] = TemplateItem.NotRequiredNotNull(typeof(double))
            })
        );

        public static readonly TemplatePacket put = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Required(new() {
                ["name"] = TemplateItem.RequiredNull(typeof(string)),
                ["public"] = TemplateItem.RequiredNotNull(typeof(bool)),
                ["initialMoney"] = TemplateItem.RequiredNotNull(typeof(double)),
                ["lostMoney"] = TemplateItem.RequiredNotNull(typeof(double)),
                ["savedMoney"] = TemplateItem.RequiredNotNull(typeof(double))
            })
        );

    }

}