using PacketHandlers;
using Controller;

public static class ManagerHelper {

    public static SendingPacket WithTokenReader(TokenController token,string? extracted_token,Func<AccessToken?, SendingPacket> action) {

        if (token._IsSystemPublic() == false)
            return SendErrors.SystemPrivate();

        AccessToken? access_token = token._GetToken(extracted_token);
        return action(access_token);

    }

    public static async Task<SendingPacket> WithTokenReaderAsync(TokenController token,string? extracted_token,Func<AccessToken?, Task<SendingPacket>> action) {

        if (token._IsSystemPublic() == false)
            return SendErrors.SystemPrivate();

        AccessToken? access_token = token._GetToken(extracted_token);
        return await action(access_token);

    }

    public static SendingPacket WithTokenWriter(TokenController token,string? extracted_token,Func<AccessToken, SendingPacket> action) {
        
        AccessToken? access_token = token._GetToken(extracted_token);
        if (AccessToken.IsValid(access_token) == false)
            return SendErrors.InvalidToken(access_token);

        if (access_token!.is_writer == false)
            return SendErrors.WriterTokenNeeded();

        return action(access_token);

    }

    public static async Task<SendingPacket> WithTokenWriterAsync(TokenController token,string? extracted_token,Func<AccessToken, Task<SendingPacket>> action) {
        
        AccessToken? access_token = token._GetToken(extracted_token);
        if (AccessToken.IsValid(access_token) == false)
            return SendErrors.InvalidToken(access_token);

        if (access_token!.is_writer == false)
            return SendErrors.WriterTokenNeeded();

        return await action(access_token);

    }

}