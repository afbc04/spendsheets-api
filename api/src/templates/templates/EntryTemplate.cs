namespace Templates {

    public static class EntryTemplate {

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
                ["categoryId"] = TemplateItem.NotRequiredNull(typeof(long)),
                ["collectionId"] = TemplateItem.NotRequiredNull(typeof(long)),
                ["visible"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                ["public"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                ["active"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                ["type"] = TemplateItem.RequiredNotNull(typeof(string)),
                ["date"] = TemplateItem.NotRequiredNotNull(typeof(DateOnly)),
                ["dueDate"] = TemplateItem.NotRequiredNull(typeof(DateOnly)),
                ["description"] = TemplateItem.NotRequiredNull(typeof(string)),
                ["targetMoney"] = TemplateItem.RequiredNull(typeof(double)),
                ["actualMoney"] = TemplateItem.RequiredNotNull(typeof(double)),
                ["status"] = TemplateItem.NotRequiredNull(typeof(string))
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
                ["categoryId"] = TemplateItem.NotRequiredNull(typeof(long)),
                ["collectionId"] = TemplateItem.NotRequiredNull(typeof(long)),
                ["visible"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                ["public"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                ["active"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                ["type"] = TemplateItem.RequiredNotNull(typeof(string)),
                ["date"] = TemplateItem.NotRequiredNotNull(typeof(DateOnly)),
                ["dueDate"] = TemplateItem.NotRequiredNull(typeof(DateOnly)),
                ["description"] = TemplateItem.NotRequiredNull(typeof(string)),
                ["targetMoney"] = TemplateItem.RequiredNull(typeof(double)),
                ["actualMoney"] = TemplateItem.RequiredNotNull(typeof(double)),
                ["status"] = TemplateItem.NotRequiredNull(typeof(string))
            })
        );

        private static readonly TemplatePacket update_partial = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Required(new() {
                ["categoryId"] = TemplateItem.NotRequiredNull(typeof(long)),
                ["collectionId"] = TemplateItem.NotRequiredNull(typeof(long)),
                ["visible"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                ["public"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                ["active"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                ["type"] = TemplateItem.NotRequiredNotNull(typeof(string)),
                ["date"] = TemplateItem.NotRequiredNotNull(typeof(DateOnly)),
                ["dueDate"] = TemplateItem.NotRequiredNull(typeof(DateOnly)),
                ["description"] = TemplateItem.NotRequiredNull(typeof(string)),
                ["targetMoney"] = TemplateItem.NotRequiredNull(typeof(double)),
                ["actualMoney"] = TemplateItem.NotRequiredNotNull(typeof(double)),
                ["status"] = TemplateItem.NotRequiredNull(typeof(string)),
                ["draft"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
                ["deleted"] = TemplateItem.NotRequiredNotNull(typeof(bool)),
            })
        );

        private static readonly TemplatePacket delete = new(
            TemplateAuth.Required(),
            TemplateQuery.Non(),
            TemplateBody.Non()
        );

    }

}
