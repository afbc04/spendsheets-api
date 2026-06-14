public static class ErrorView
{
    public static Dictionary<string,object?> ToView(ErrorCategory id, string message, Dictionary<string,object?>? body)
    {
        var res = new Dictionary<string,object?>(){
            ["error"] = id.ToString(),
            ["message"] = message
        };

        if (body is not null)
            res["body"] = body;

        return res;
    }
}