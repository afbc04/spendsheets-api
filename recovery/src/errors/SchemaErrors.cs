public class SchemaException(ErrorCategory errorId) : Exception()
{
    public ErrorCategory ErrorId { get; } = errorId;
}