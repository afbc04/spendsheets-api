namespace Templates {

    public abstract class TemplateField {

        public bool is_required { get; protected set; }
        public bool is_list { get; protected set; }
        public bool allow_null { get; protected set; }

    }

    public class TemplateItem : TemplateField {

        public Type datatype { get; set; }

        public TemplateItem(bool is_required, Type datatype, bool is_list, bool allow_null) {
            this.is_required = is_required;
            this.datatype = datatype;
            this.is_list = is_list;
            this.allow_null = allow_null;
        }

        public static TemplateItem RequiredNotNull(Type datatype) =>
            new TemplateItem(true,datatype,false,false);

        public static TemplateItem RequiredNull(Type datatype) =>
            new TemplateItem(true,datatype,false,true);

        public static TemplateItem NotRequiredNotNull(Type datatype) =>
            new TemplateItem(false,datatype,false,false);

        public static TemplateItem NotRequiredNull(Type datatype) =>
            new TemplateItem(false,datatype,false,true);

        public static TemplateItem RequiredNotNullList(Type datatype) =>
            new TemplateItem(true,datatype,true,false);

        public static TemplateItem RequiredNullList(Type datatype) =>
            new TemplateItem(true,datatype,true,true);

        public static TemplateItem NotRequiredNotNullList(Type datatype) =>
            new TemplateItem(false,datatype,true,false);

        public static TemplateItem NotRequiredNullList(Type datatype) =>
            new TemplateItem(false,datatype,true,true);

    }

    public class TemplateObject : TemplateField {

        public Dictionary<string, TemplateField> obj { get; set; }

        public TemplateObject(bool is_required, bool is_list, bool allow_null, Dictionary<string, TemplateField> obj) {
            this.is_required = is_required;
            this.is_list = is_list;
            this.allow_null = allow_null;
            this.obj = obj;
        }

        public static TemplateObject RequiredNotNull(Dictionary<string, TemplateField> obj) =>
            new TemplateObject(true,false,false,obj);

        public static TemplateObject RequiredNull(Dictionary<string, TemplateField> obj) =>
            new TemplateObject(true,false,true,obj);

        public static TemplateObject NotRequiredNotNull(Dictionary<string, TemplateField> obj) =>
            new TemplateObject(false,false,false,obj);

        public static TemplateObject NotRequiredNull(Dictionary<string, TemplateField> obj) =>
            new TemplateObject(false,false,true,obj);

        public static TemplateObject RequiredNotNullList(Dictionary<string, TemplateField> obj) =>
            new TemplateObject(true,true,false,obj);

        public static TemplateObject RequiredNullList(Dictionary<string, TemplateField> obj) =>
            new TemplateObject(true,true,true,obj);

        public static TemplateObject NotRequiredNotNullList(Dictionary<string, TemplateField> obj) =>
            new TemplateObject(false,true,false,obj);

        public static TemplateObject NotRequiredNullList(Dictionary<string, TemplateField> obj) =>
            new TemplateObject(false,true,true,obj);

    }

}