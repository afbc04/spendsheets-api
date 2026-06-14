public static class ValidatorTokenMiddleware
{
    private static readonly int tokenHeaderSubstring = "Bearer ".Length;

    public static async Task<string> ValidateToken(HttpRequest request)
    {
        return (await _GetToken(request, true))!;
    }

    public static async Task<string?> TryExtractToken(HttpRequest request)
    {
        return await _GetToken(request, false);
    }

    public static async Task<string?> _GetToken(HttpRequest request, bool isRequired)
    {
        try
        {
            if (request.Headers.TryGetValue("Authorization", out var authHeaderValue) == false)
                throw new ValidatorTokenConfigMiddlewareException(ValidatorTokenConfigMiddlewareExceptionEnum.TokenRequired);

            string authHeader = authHeaderValue.ToString();

            if (authHeader.StartsWith($"Bearer ") == false)
                throw new ValidatorTokenConfigMiddlewareException(ValidatorTokenConfigMiddlewareExceptionEnum.InvalidAuthType);

            string tokenExtracted = authHeader[tokenHeaderSubstring..];
            Token? token = Authenticator.GetToken();

            if (token is null || token.IsExpired())
                throw new ValidatorTokenConfigMiddlewareException(ValidatorTokenConfigMiddlewareExceptionEnum.TokenExpired);

            if (!token.IsValid(tokenExtracted))
                throw new ValidatorTokenConfigMiddlewareException(ValidatorTokenConfigMiddlewareExceptionEnum.TokenInvalid);

            return tokenExtracted;
        }
        catch (ValidatorTokenConfigMiddlewareException)
        {
            if (isRequired)
                throw;

            return null;
        }
    }
} 
