using PacketHandlers;

public static class SendErrors {

    public static SendingPacket ConfigNotExists() =>
        new PacketFail(404,"System is not configured",new Dictionary<string,object> {
            ["createConfig"] = new Dictionary<string,object> {
                ["endpoint"] = "/v1.0/config",
                ["method"] = "POST"
            }
        });

    public static SendingPacket ConfigPrivate() =>
        new PacketFail(403,"System is private. You cannot see system's information without access token");
    
    public static SendingPacket InvalidToken() =>
        new PacketFail(401,"Token is not valid or expired");
    

    public static SendingPacket InvalidToken(AccessToken? token) =>
        token == null ? new PacketFail(401,"Token is not valid") : new PacketFail(401,"Token is expired");
        

    public static SendingPacket WriterTokenNeeded() =>
        new PacketFail(403,"This endpoint requires a token in writer mode");
    
    public static SendingPacket EntryDoesNotExists() =>
        new PacketFail(404,"Entry does not exists");

    public static SendingPacket EntryDoesNotSupportMovements() =>
        new PacketFail(403,"Entry does not support movements");

}