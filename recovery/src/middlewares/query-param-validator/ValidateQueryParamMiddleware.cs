public static class ValidatorQueryParamMiddleware
{
    public static async Task<long> ValidateNumericalID(string queryParam)
    {
        if (long.TryParse(queryParam, out long number))
            return number;
        else
            throw new ValidatorQueryParamMiddlewareException("ID must be numerical");
    }
} 
