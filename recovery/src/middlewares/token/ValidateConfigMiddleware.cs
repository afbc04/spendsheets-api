public static class ValidatorConfigMiddleware
{
    public static async Task VerifyIfUserExists()
    {
        var _ = await UserConfiguration.GetUser() ?? throw new ValidatorTokenConfigMiddlewareException(ValidatorTokenConfigMiddlewareExceptionEnum.UserDoesntExists);
    }
} 
