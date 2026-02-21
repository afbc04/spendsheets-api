public static class TokenResponse {

    public static IDictionary<string,object?> ToJson(Token token, bool is_writer) =>
        new Dictionary<string,object?> {
            ["accessToken"] = token.token,
            ["tokenType"] = "Bearer",
            ["expiresIn"] = (long) (token._expiration_time - DateTime.UtcNow).TotalMinutes,
            ["writer"] = is_writer
        };


}