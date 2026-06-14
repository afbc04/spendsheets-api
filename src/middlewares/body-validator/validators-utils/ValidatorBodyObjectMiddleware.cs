public class ValidatorBodyObjectMiddleware : ValidatorBodyFieldMiddleware
{

    public Dictionary<string, ValidatorBodyFieldMiddleware> obj { get; set; }

    public ValidatorBodyObjectMiddleware(bool is_required, bool is_list, bool allow_null, Dictionary<string, ValidatorBodyFieldMiddleware> obj)
    {
        this.IsRequired = is_required;
        this.IsList = is_list;
        this.AllowNull = allow_null;
        this.obj = obj;
    }

    public static ValidatorBodyObjectMiddleware RequiredNotNull(Dictionary<string, ValidatorBodyFieldMiddleware> obj) =>
        new ValidatorBodyObjectMiddleware(true, false, false, obj);

    public static ValidatorBodyObjectMiddleware RequiredNull(Dictionary<string, ValidatorBodyFieldMiddleware> obj) =>
        new ValidatorBodyObjectMiddleware(true, false, true, obj);

    public static ValidatorBodyObjectMiddleware NotRequiredNotNull(Dictionary<string, ValidatorBodyFieldMiddleware> obj) =>
        new ValidatorBodyObjectMiddleware(false, false, false, obj);

    public static ValidatorBodyObjectMiddleware NotRequiredNull(Dictionary<string, ValidatorBodyFieldMiddleware> obj) =>
        new ValidatorBodyObjectMiddleware(false, false, true, obj);

    public static ValidatorBodyObjectMiddleware RequiredNotNullList(Dictionary<string, ValidatorBodyFieldMiddleware> obj) =>
        new ValidatorBodyObjectMiddleware(true, true, false, obj);

    public static ValidatorBodyObjectMiddleware RequiredNullList(Dictionary<string, ValidatorBodyFieldMiddleware> obj) =>
        new ValidatorBodyObjectMiddleware(true, true, true, obj);

    public static ValidatorBodyObjectMiddleware NotRequiredNotNullList(Dictionary<string, ValidatorBodyFieldMiddleware> obj) =>
        new ValidatorBodyObjectMiddleware(false, true, false, obj);

    public static ValidatorBodyObjectMiddleware NotRequiredNullList(Dictionary<string, ValidatorBodyFieldMiddleware> obj) =>
        new ValidatorBodyObjectMiddleware(false, true, true, obj);

}