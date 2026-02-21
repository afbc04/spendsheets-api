namespace Templates {

    public static class CollectionTemplate {

        public static TemplatePacket List() => list;
        public static TemplatePacket Create() => create;
        public static TemplatePacket Clear() => clear;
        public static TemplatePacket Map() => map;
        public static TemplatePacket Get() => get;
        public static TemplatePacket UpdateFull() => update_full;
        public static TemplatePacket UpdatePartial() => update_partial;
        public static TemplatePacket Delete() => delete;



        private static readonly TemplatePacket list = new(
            TemplateAuth.NotRequired(),
            TemplateQuery.PageAndSortsAndQueries(
                
                new() {
                    ["id"] = TemplateQuerySortItem.Default(),
                    ["name"] = TemplateQuerySortItem.HiddenCaseInsensitive()
                },

                new() {
                    ["name"] = TemplateQueryItem.Item(typeof(string)),
                    ["active"] = TemplateQueryItem.Item(typeof(bool)),
                    ["monthlyService"] = TemplateQueryItem.Item(typeof(bool))
                }

            ),
            TemplateBody.Non()
        );

        private static readonly TemplatePacket create = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Required(new() {
                ["name"] = TemplateItem.RequiredNotNull(typeof(string)),
                ["description"] = TemplateItem.NotRequiredNull(typeof(string)),
                ["monthlyService"] = TemplateObject.NotRequiredNull(
                    new() {
                        ["category"] = TemplateItem.NotRequiredNull(typeof(long)),
                        ["moneyAmount"] = TemplateItem.NotRequiredNull(typeof(double)),
                        ["active"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                    }
                )
            })
        );

        private static readonly TemplatePacket clear = new(
            TemplateAuth.Required(),
            TemplateQuery.OnlyQueries(
            
                new() {
                    ["ids"] = TemplateQueryItem.Item(typeof(string)),
                    ["monthlyService"] = TemplateQueryItem.Item(typeof(bool)),
                    ["active"] = TemplateQueryItem.Item(typeof(bool))
                }

            ),
            TemplateBody.Non()
        );

        private static readonly TemplatePacket map = new(
            TemplateAuth.Required(),
            TemplateQuery.OnlyQueries(
            
                new() {
                    ["ids"] = TemplateQueryItem.Item(typeof(string)),
                    ["active"] = TemplateQueryItem.Item(typeof(bool))
                }

            ),
            TemplateBody.Required(

                new() {
                    ["monthlyServiceActive"] = TemplateItem.RequiredNotNull(typeof(bool))
                }

            )
        );

        private static readonly TemplatePacket get = new(
            TemplateAuth.NotRequired(),
            TemplateQuery.Non(),
            TemplateBody.Non()
        );

        private static readonly TemplatePacket update_full = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Required(new() {
                ["name"] = TemplateItem.RequiredNotNull(typeof(string)),
                ["description"] = TemplateItem.NotRequiredNull(typeof(string)),
                ["monthlyService"] = TemplateObject.NotRequiredNull(
                    new() {
                        ["category"] = TemplateItem.NotRequiredNull(typeof(long)),
                        ["moneyAmount"] = TemplateItem.NotRequiredNull(typeof(double)),
                        ["active"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                    }
                )
            })
        );

        private static readonly TemplatePacket update_partial = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Required(new() {
                ["name"] = TemplateItem.NotRequiredNotNull(typeof(string)),
                ["description"] = TemplateItem.NotRequiredNull(typeof(string)),
                ["monthlyService"] = TemplateObject.NotRequiredNull(
                    new() {
                        ["category"] = TemplateItem.NotRequiredNull(typeof(long)),
                        ["moneyAmount"] = TemplateItem.NotRequiredNull(typeof(double)),
                        ["active"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                    }
                )
            })
        );

        private static readonly TemplatePacket delete = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Non()
        );

    }

}
