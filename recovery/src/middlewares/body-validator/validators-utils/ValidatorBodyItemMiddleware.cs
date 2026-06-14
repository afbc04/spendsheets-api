public class ValidatorBodyItemMiddleware : ValidatorBodyFieldMiddleware
{

    public ValidatorBodyFieldTypeMiddleware DateType { get; set; }

    public ValidatorBodyItemMiddleware(bool is_required, ValidatorBodyFieldTypeMiddleware datatype, bool is_list, bool allow_null)
    {
        this.IsRequired = is_required;
        this.DateType = datatype;
        this.IsList = is_list;
        this.AllowNull = allow_null;
    }

    public static ValidatorBodyItemMiddleware RequiredNotNull(ValidatorBodyFieldTypeMiddleware datatype) =>
        new ValidatorBodyItemMiddleware(true, datatype, false, false);

    public static ValidatorBodyItemMiddleware RequiredNull(ValidatorBodyFieldTypeMiddleware datatype) =>
        new ValidatorBodyItemMiddleware(true, datatype, false, true);

    public static ValidatorBodyItemMiddleware NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware datatype) =>
        new ValidatorBodyItemMiddleware(false, datatype, false, false);

    public static ValidatorBodyItemMiddleware NotRequiredNull(ValidatorBodyFieldTypeMiddleware datatype) =>
        new ValidatorBodyItemMiddleware(false, datatype, false, true);

    public static ValidatorBodyItemMiddleware RequiredNotNullList(ValidatorBodyFieldTypeMiddleware datatype) =>
        new ValidatorBodyItemMiddleware(true, datatype, true, false);

    public static ValidatorBodyItemMiddleware RequiredNullList(ValidatorBodyFieldTypeMiddleware datatype) =>
        new ValidatorBodyItemMiddleware(true, datatype, true, true);

    public static ValidatorBodyItemMiddleware NotRequiredNotNullList(ValidatorBodyFieldTypeMiddleware datatype) =>
        new ValidatorBodyItemMiddleware(false, datatype, true, false);

    public static ValidatorBodyItemMiddleware NotRequiredNullList(ValidatorBodyFieldTypeMiddleware datatype) =>
        new ValidatorBodyItemMiddleware(false, datatype, true, true);

}