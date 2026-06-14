public static class ValidatorSessionMiddleware
{
    private static readonly int tokenHeaderSubstring = "Bearer ".Length;

    public static async Task<Session> ValidateSessionAdmin(HttpRequest request)
    {
        var session = (await _GetToken(request, true))!;
        if (!session.Profile.IsAdmin)
                throw new ValidatorSessionException("Only administrators can perform this action", 403);

        return session;
    }

    public static async Task<Session> ValidateSessionAdminOrSelf(HttpRequest request, string username)
    {
        var session = (await _GetToken(request, true))!;
        if (!session.Profile.IsAdmin && session.Profile.Username != username)
                throw new ValidatorSessionException("You can't perform this action", 403);

        return session;
    }

    public static async Task<Session> ValidateSession(HttpRequest request)
    {
        return (await _GetToken(request, true))!;
    }

    public static async Task<Session?> TryValidateSession(HttpRequest request)
    {
        return await _GetToken(request, false);
    }

    private static async Task<Session?> _GetToken(HttpRequest request, bool isRequired)
    {
        try
        {
            if (request.Headers.TryGetValue("Authorization", out var authHeaderValue) == false)
                throw new ValidatorSessionException("Request does not have authorization header", 400);

            string authHeader = authHeaderValue.ToString();

            if (authHeader.StartsWith($"Bearer ") == false)
                throw new ValidatorSessionException("Authorization should be 'Bearer'", 400);

            string tokenExtracted = authHeader[tokenHeaderSubstring..];
            Session? session = SessionManager.GetSession(tokenExtracted);

            if (session is null)
                throw new ValidatorSessionException("Session does not exists", 401);

            if (!session.IsValid())
                throw new ValidatorSessionException("Session is expired", 401);

            session.Refresh();
            return session;
        }
        catch (ValidatorSessionException)
        {
            if (isRequired)
                throw;

            return null;
        }
    }
} 
