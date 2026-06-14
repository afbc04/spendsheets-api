public static class ValidatorTokenConfigErrorMiddleware
{
    public static async Task<SendingPacket> Handle(HttpContext context, ValidatorTokenConfigMiddlewareException ex)
    {
        context.Response.ContentType = "application/json";

        switch (ex.Type)
        {
            case ValidatorTokenConfigMiddlewareExceptionEnum.UserDoesntExists:
                return SendingPacket.Error(ErrorCategory.CONFIG_USER_NOT_EXISTING);

            case ValidatorTokenConfigMiddlewareExceptionEnum.TokenRequired:
                return SendingPacket.Error(ErrorCategory.TOKEN_REQUIRED);

            case ValidatorTokenConfigMiddlewareExceptionEnum.TokenInvalid:
                return SendingPacket.Error(ErrorCategory.TOKEN_INVALID);

            case ValidatorTokenConfigMiddlewareExceptionEnum.TokenExpired:
                return SendingPacket.Error(ErrorCategory.TOKEN_EXPIRED);

            case ValidatorTokenConfigMiddlewareExceptionEnum.InvalidAuthType:
                return SendingPacket.Error(ErrorCategory.TOKEN_INVALID_AUTH);

            default:
                return SendingPacket.Error(ErrorCategory.CONFIG_EXCEPTION);
        }
    }
}