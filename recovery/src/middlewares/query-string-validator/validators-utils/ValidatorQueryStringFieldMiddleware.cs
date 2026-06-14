public enum ValidatorQueryStringFieldTypeMiddleware
{
    Integer,
    Float,
    String,
    Date,
    DateTime,
    Boolean
}

public abstract class ValidatorQueryStringFieldMiddleware
{
    public bool IsList { get; protected set; }
    public bool AllowNull { get; protected set; }

    public static string GetTypeName(ValidatorQueryStringFieldTypeMiddleware type, bool canBeNull)
    {
        string res = type switch
        {
            ValidatorQueryStringFieldTypeMiddleware.Integer => "integer",
            ValidatorQueryStringFieldTypeMiddleware.Float => "float",
            ValidatorQueryStringFieldTypeMiddleware.String => "string",
            ValidatorQueryStringFieldTypeMiddleware.Date => "date",
            ValidatorQueryStringFieldTypeMiddleware.DateTime => "datetime",
            ValidatorQueryStringFieldTypeMiddleware.Boolean => "boolean",
            _ => "???"
        };
        
        return canBeNull ? $"{res}?" : res;
    }
}