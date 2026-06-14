public class SendingPacket
{
    public int StatusCode {get; private set;}
    public object? Body {get; private set;}

    private SendingPacket(int statusCode, object? body)
    {
        StatusCode = statusCode;
        Body = body;
    }

    public static SendingPacket Error(int statusCode, string message, Dictionary<string,object?>? extraMessage = null)
    {
        var body = new Dictionary<string, object?>
        {
            ["message"] = message
        };
        
        if (extraMessage is not null)
            foreach(var kp in extraMessage)
                body[kp.Key] = kp.Value;

        return new SendingPacket(statusCode, body);
    }

    public static SendingPacket Success(int statusCode, object? body = null) =>
        new(statusCode, body);

    public IResult Send()
    {
        return this.Body is null
            ? Results.StatusCode(this.StatusCode)
            : Results.Json(this.Body, statusCode: this.StatusCode);
    }
}