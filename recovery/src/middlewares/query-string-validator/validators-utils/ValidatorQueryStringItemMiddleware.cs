public class ValidatorQueryStringItemMiddleware : ValidatorQueryStringFieldMiddleware
{

    public ValidatorQueryStringFieldTypeMiddleware DateType { get; set; }

    public ValidatorQueryStringItemMiddleware(ValidatorQueryStringFieldTypeMiddleware datatype, bool isList, bool allowNull)
    {
        this.DateType = datatype;
        this.IsList = isList;
        this.AllowNull = allowNull;
    }

    public static ValidatorQueryStringItemMiddleware NotNull(ValidatorQueryStringFieldTypeMiddleware datatype) =>
        new ValidatorQueryStringItemMiddleware(datatype, false, false);

    public static ValidatorQueryStringItemMiddleware Null(ValidatorQueryStringFieldTypeMiddleware datatype) =>
        new ValidatorQueryStringItemMiddleware(datatype, false, true);

    public static ValidatorQueryStringItemMiddleware NotNullList(ValidatorQueryStringFieldTypeMiddleware datatype) =>
        new ValidatorQueryStringItemMiddleware(datatype, true, false);

    public static ValidatorQueryStringItemMiddleware NullList(ValidatorQueryStringFieldTypeMiddleware datatype) =>
        new ValidatorQueryStringItemMiddleware(datatype, true, true);

}