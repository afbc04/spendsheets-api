using PacketHandlers;
using Controller;

public static class ManagerHelper {

    public static async Task<SendingPacket> CheckConfig(ConfigController config, Func<Task<SendingPacket>> action) {

        if (config._ConfigExists() == false)
            return SendErrors.ConfigNotExists();

        return await action();

    }

    public static async Task<SendingPacket> WithTokenReaderPublic(ConfigController config,TokenController token,string? extracted_token,Func<AccessToken?, Task<SendingPacket>> action) {
        
        if (config._ConfigExists() == false)
            return SendErrors.ConfigNotExists();

        if (config._IsPublic() == false)
            return SendErrors.ConfigPrivate();

        AccessToken? access_token = token._GetToken(extracted_token);
        return await action(access_token);

    }

    public static async Task<SendingPacket> WithTokenReaderPrivate(ConfigController config,TokenController token,string? extracted_token,Func<AccessToken?, Task<SendingPacket>> action) {
        
        if (config._ConfigExists() == false)
            return SendErrors.ConfigNotExists();

        AccessToken? access_token = token._GetToken(extracted_token);
        if (AccessToken.IsValid(access_token) == false)
            return SendErrors.InvalidToken(access_token);

        return await action(access_token);

    }

    public static async Task<SendingPacket> WithTokenWriter(ConfigController config,TokenController token,string? extracted_token,Func<AccessToken, Task<SendingPacket>> action) {
        
        if (config._ConfigExists() == false)
            return SendErrors.ConfigNotExists();

        AccessToken? access_token = token._GetToken(extracted_token);
        if (AccessToken.IsValid(access_token) == false)
            return SendErrors.InvalidToken(access_token);

        if (access_token!.is_writer == false)
            return SendErrors.WriterTokenNeeded();

        return await action(access_token);

    }

}