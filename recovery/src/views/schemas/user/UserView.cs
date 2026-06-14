public static class UserView
{
    public static Dictionary<string,object?> ToView(User user, bool hidden)
        => hidden 
            ? ViewifyHide(user.Name, user.CreationDate)
            : ViewifyShow(user.Username, user.Name, user.InitialMoney, user.CreationDate);

    private static Dictionary<string,object?> ViewifyShow(string username, string? name, long initialMoney, DateOnly createdAt)
    {
        return new Dictionary<string,object?>(){
            ["username"] = username,
            ["name"] = name,
            ["initialMoney"] = initialMoney,
            ["createdAt"] = createdAt,
            ["hidden"] = false
        };
    }

    private static Dictionary<string,object?> ViewifyHide(string? name, DateOnly createdAt)
    {
        return new Dictionary<string,object?>(){
            ["username"] = "???",
            ["name"] = name,
            ["initialMoney"] = null,
            ["createdAt"] = createdAt,
            ["hidden"] = true
        };
    }

}