public enum ValidatorBodyFieldTypeMiddleware
{
    Integer,
    Float,
    String,
    Date,
    DateTime,
    Boolean
}

public abstract class ValidatorBodyFieldMiddleware
{
    public bool IsRequired { get; protected set; }
    public bool IsList { get; protected set; }
    public bool AllowNull { get; protected set; }

    public static string GetTypeName(ValidatorBodyFieldTypeMiddleware type, bool canBeNull)
    {
        string res = type switch
        {
            ValidatorBodyFieldTypeMiddleware.Integer => "integer",
            ValidatorBodyFieldTypeMiddleware.Float => "float",
            ValidatorBodyFieldTypeMiddleware.String => "string",
            ValidatorBodyFieldTypeMiddleware.Date => "date",
            ValidatorBodyFieldTypeMiddleware.DateTime => "datetime",
            ValidatorBodyFieldTypeMiddleware.Boolean => "boolean",
            _ => "???"
        };
        
        return canBeNull ? $"{res}?" : res;
    }
}