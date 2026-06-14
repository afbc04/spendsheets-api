public static class ProfileView
{
    public static Dictionary<string,object?> ToView(Session session, bool hidden)
        => ViewifyShow(session.Profile.Username, session.Profile.Name, session.Profile.CreationDate, session.Profile.IsAdmin, session.Profile.IsActive, session.Profile.InactiveDate, session.IsValid(), session.LastUpdated);

    private static Dictionary<string,object?> ViewifyShow(string username, string? name, DateOnly createdAt, bool isAdmin, bool isActive, DateOnly? inactiveDate, bool sessionActive, DateTime? lastTimeSession)
    {
        return new Dictionary<string,object?>(){
            ["username"] = username,
            ["name"] = name,
            ["createdAt"] = createdAt,
            ["admin"] = isAdmin,
            ["active"] = isActive,
            ["inactiveDate"] = inactiveDate,
            ["sessionActive"] = sessionActive,
            ["lastTimeSession"] = lastTimeSession,
            ["hidden"] = false
        };
    }
}