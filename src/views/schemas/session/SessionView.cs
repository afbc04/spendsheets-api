public static class SessionView
{
    public static Dictionary<string,object?> ToView(Session session)
    {
        return new Dictionary<string,object?>(){
            ["sessionToken"] = session.Token,
            ["profile"] = session.Profile.Username,
            ["lifeTime"] = Session.ExpirationMinutes,
        };
    }
}