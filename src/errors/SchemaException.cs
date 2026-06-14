public class SchemaException(string message, int statusCode = 400) : Exception()
{
    public string message { get; } = message;
    public int statusCode { get; } = statusCode;
}