public enum ValidatorQueryStringMiddlewareExceptionEnum
{
    WrongDatatypeField,
    NotValidSortField,
    PageMin,
    LimitMin,
    LimitMax,
    InvalidPage,
    InvalidLimit,
    InvalidSortQuery
}

public class ValidatorQueryStringMiddlewareException : Exception
{
    public ValidatorQueryStringMiddlewareExceptionEnum Type { get; }
    public Dictionary<string,string> WrongDatatypeFields { get; }
    public List<string> PageSortInvalidFields { get; }

    public ValidatorQueryStringMiddlewareException(ValidatorQueryStringMiddlewareExceptionEnum type)
    {
        Type = type;
        PageSortInvalidFields= [];
        WrongDatatypeFields = [];
    }

    public ValidatorQueryStringMiddlewareException(ValidatorQueryStringMiddlewareExceptionEnum type, Dictionary<string,string> wrongDatatypes)
    {
        Type = type;
        PageSortInvalidFields = [];
        WrongDatatypeFields = wrongDatatypes;
    }

    public ValidatorQueryStringMiddlewareException(ValidatorQueryStringMiddlewareExceptionEnum type, List<string> pageSortInvalidFields)
    {
        Type = type;
        PageSortInvalidFields = pageSortInvalidFields;
        WrongDatatypeFields = [];
    }
}