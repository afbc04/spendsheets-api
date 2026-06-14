public class ValidatorSessionException(string message, int statusCode) : Exception()
{
    public string message { get; } = message;
    public int statusCode { get; } = statusCode;
}