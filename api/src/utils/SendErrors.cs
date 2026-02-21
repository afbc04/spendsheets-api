using PacketHandlers;

public static class SendErrors {

    public static SendingPacket SystemPrivate() =>
        new PacketFail(403,"System is private. You cannot see system's information without access token");
    
    public static SendingPacket InvalidToken() =>
        new PacketFail(401,"Token is not valid or expired");
    

    public static SendingPacket InvalidToken(AccessToken? token) =>
        token == null ? new PacketFail(401,"Token is not valid") : new PacketFail(401,"Token is expired");
        

    public static SendingPacket WriterTokenNeeded() =>
        new PacketFail(403,"This endpoint requires a writer access token");
    
    public static SendingPacket EntryDoesNotExists() =>
        new PacketFail(404,"Entry does not exists");

    public static SendingPacket EntryDoesNotSupportMovements() =>
        new PacketFail(403,"Entry does not support movements");

}