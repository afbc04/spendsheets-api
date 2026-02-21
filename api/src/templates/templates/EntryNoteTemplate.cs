namespace Templates {

    public static class EntryNoteTemplate {

        public static TemplatePacket List() => list;
        public static TemplatePacket Create() => create;
        //public static TemplatePacket Clear() => clear;
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
                    ["name"] = TemplateQueryItem.Item(typeof(string))
                }

            ),
            TemplateBody.Non()
        );

        private static readonly TemplatePacket create = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Required(new() {
                ["date"] = TemplateItem.NotRequiredNotNull(typeof(DateOnly)),
                ["note"] = TemplateItem.NotRequiredNull(typeof(string)),
                ["money"] = TemplateItem.NotRequiredNull(typeof(double)),
            })
        );

        private static readonly TemplatePacket clear = new(
            TemplateAuth.Required(),
            TemplateQuery.OnlyQueries(
            
                new() {
                    ["ids"] = TemplateQueryItem.Item(typeof(string))
                }

            ),
            TemplateBody.Non()
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
                ["date"] = TemplateItem.NotRequiredNotNull(typeof(DateOnly)),
                ["note"] = TemplateItem.NotRequiredNull(typeof(string)),
                ["money"] = TemplateItem.NotRequiredNull(typeof(double)),
            })
        );

        private static readonly TemplatePacket update_partial = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Required(new() {
                ["date"] = TemplateItem.NotRequiredNotNull(typeof(DateOnly)),
                ["note"] = TemplateItem.NotRequiredNull(typeof(string)),
                ["money"] = TemplateItem.NotRequiredNull(typeof(double)),
            })
        );

        private static readonly TemplatePacket delete = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Non()
        );

    }

}
