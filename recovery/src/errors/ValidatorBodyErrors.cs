public enum ValidatorBodyMiddlewareExceptionEnum
{
    RequiredBody,
    NotJSON,
    InvalidJSON,
    RequiredFieldMissing,
    WrongDatatypeField
}

public class ValidatorBodyMiddlewareException : Exception
{
    public ValidatorBodyMiddlewareExceptionEnum Type { get; }
    public List<string> RequiredFieldsMissing { get; }
    public Dictionary<string,string> WrongDatatypeFields { get; }

    public ValidatorBodyMiddlewareException(ValidatorBodyMiddlewareExceptionEnum type)
    {
        Type = type;
        RequiredFieldsMissing = [];
        WrongDatatypeFields = [];
    }

    public ValidatorBodyMiddlewareException(ValidatorBodyMiddlewareExceptionEnum type, List<string> requiredFieldsMissing)
    {
        Type = type;
        RequiredFieldsMissing = requiredFieldsMissing;
        WrongDatatypeFields = [];
    }

    public ValidatorBodyMiddlewareException(ValidatorBodyMiddlewareExceptionEnum type, Dictionary<string,string> wrongDatatypes)
    {
        Type = type;
        RequiredFieldsMissing = [];
        WrongDatatypeFields = wrongDatatypes;
    }
}