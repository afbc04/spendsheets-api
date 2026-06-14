public static class TokenView
{
    public static Dictionary<string,object?> ToView(Token token)
    {
        return new Dictionary<string,object?>(){
            ["accessToken"] = token.AccessToken,
            ["refreshToken"] = token.RefreshToken,
            ["expiresIn"] = token.AccessTokenExpirationDate,
            ["authType"] = "Bearer"
        };
    }
}