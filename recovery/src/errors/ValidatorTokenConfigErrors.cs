public enum ValidatorTokenConfigMiddlewareExceptionEnum
{
    UserDoesntExists,
    TokenRequired,
    TokenInvalid,
    TokenExpired,
    InvalidAuthType
}

public class ValidatorTokenConfigMiddlewareException(ValidatorTokenConfigMiddlewareExceptionEnum type) : Exception()
{
    public ValidatorTokenConfigMiddlewareExceptionEnum Type { get; } = type;
}