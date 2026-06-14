public static class TokenController
{/*
    public static async Task<SendingPacket> ValidateToken(string token)
    {
        Token? tokenExtracted = Authenticator.GetToken();

        if (tokenExtracted is null)
            return SendingPacket.Error(ErrorCategory.TOKEN_EXPIRED);

        if (!tokenExtracted.IsValid(token))
            return SendingPacket.Error(ErrorCategory.TOKEN_INVALID);

        if (tokenExtracted.IsExpired())
            return SendingPacket.Error(ErrorCategory.TOKEN_EXPIRED);

        return SendingPacket.Success(204);
    }

    public static async Task<SendingPacket> RenewToken(Dictionary<string,object?> tokenData)
    {
        switch ((string) tokenData["grantType"]!)
        {
            case "password":

                if (!tokenData.TryGetValue("username", out object? username) || !tokenData.TryGetValue("password", out object? password))
                    return SendingPacket.Error(ErrorCategory.TOKEN_GRANT_TYPE_PASSWORD_MISSING_REQUIRED_FIELDS);

                User user = (await UserConfiguration.GetUser())!;

                if (user.Username != (string)username! || user.VerifyPassword((string)password!))
                    return SendingPacket.Error(ErrorCategory.TOKEN_INVALID_CREDENTIALS);

                Token tokenViaPassword = Authenticator.GenerateToken();
                return SendingPacket.Success(201,TokenView.ToView(tokenViaPassword));

            case "refreshToken":

                if (!tokenData.TryGetValue("refreshToken", out object? refreshToken))
                        return SendingPacket.Error(ErrorCategory.TOKEN_GRANT_TYPE_REFRESH_TOKEN_MISSING_REQUIRED_FIELDS);
                
                Token? tokenExtracted = Authenticator.GetToken();

                if (tokenExtracted is null || tokenExtracted.IsRefreshExpired())
                    return SendingPacket.Error(ErrorCategory.TOKEN_EXPIRED);

                if (!tokenExtracted.IsRefreshValid((string)refreshToken!))
                    return SendingPacket.Error(ErrorCategory.TOKEN_INVALID);

                Token tokenViaRefresh = Authenticator.GenerateToken();
                return SendingPacket.Success(201,TokenView.ToView(tokenViaRefresh));

            default:
                return SendingPacket.Error(ErrorCategory.TOKEN_INVALID_GRANT_TYPE);
        }
    }

    public static async Task<SendingPacket> DeleteToken(string token)
    {
       Token? tokenExtracted = Authenticator.GetToken();

        if (tokenExtracted is null || !tokenExtracted.IsValid(token))
            return SendingPacket.Error(ErrorCategory.TOKEN_INVALID);

        Authenticator.RevokeToken();
        return SendingPacket.Success(200,new Dictionary<string,object?>
            { 
                ["message"] = "Token was revoked"
            }
        );
    }*/
}