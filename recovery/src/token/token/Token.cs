public class Token(string accessToken, DateTime accessTokenExpirationDate, string refreshToken, DateTime refreshDateExpirationDate)
{
    public string AccessToken { get; set; } = accessToken;
    public DateTime AccessTokenExpirationDate { get; set; } = accessTokenExpirationDate;

    public string RefreshToken { get; set; } = refreshToken;
    public DateTime RefreshTokenExpirationDate { get; set; } = refreshDateExpirationDate;

    public bool IsExpired()
    {
        return DateTime.UtcNow > AccessTokenExpirationDate;
    }

    public bool IsValid(string tokenGiven)
    {
        return AccessToken == tokenGiven;
    }

    public bool IsRefreshExpired()
    {
        return DateTime.UtcNow > RefreshTokenExpirationDate;
    }

    public bool IsRefreshValid(string tokenGiven)
    {
        return RefreshToken == tokenGiven;
    }
}