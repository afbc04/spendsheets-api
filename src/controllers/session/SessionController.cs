public static class SessionController
{
    public static async Task<SendingPacket> GetSession(string token)
    {
        var session = SessionManager.GetSession(token);
        return session is not null && session.IsValid()
            ? SendingPacket.Success(200, new Dictionary<string,object?>() { ["message"] = "Session is valid" })
            : SendingPacket.Error(401, "Session is invalid or expired");
    }
    
    public static async Task<SendingPacket> ObtainSession(Dictionary<string, object?> sessionData)
    {
        try
        {
            string username = (string)sessionData["username"]!;
            string password = (string)sessionData["password"]!;

            var sessionOfProfile = SessionManager.GetProfile(username);
            if (sessionOfProfile is null)
                return SendingPacket.Error(404, "Profile does not exists");

            if (!sessionOfProfile.Profile.VerifyPassword(password))
                return SendingPacket.Error(403, "Invalid password of profile");

            var session = SessionManager.CreateSession(username);
            if (session is null)
                return SendingPacket.Error(422, "Error while creating session");

            return SendingPacket.Success(200, SessionView.ToView(session));
        }
        catch (SchemaException ex)
        {
            return SendingPacket.Error(ex.statusCode, ex.message);
        }
    }

    public static async Task<SendingPacket> RevokeSession(string token)
    {
        var wasRevoked = SessionManager.RevokeSession(token);
        return wasRevoked
            ? SendingPacket.Success(200, new Dictionary<string,object?>() { ["message"] = "Session was revoked" })
            : SendingPacket.Error(422, "Session couldn't be revoked");
    }
}