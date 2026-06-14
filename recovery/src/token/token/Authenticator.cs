using System.Collections.Concurrent;
using System.Security.Cryptography;

public static class Authenticator
{
    private static readonly ReaderWriterLockSlim _lock = new();
    private static Token? token = null;

    // Token Config
    private static readonly int minutesAccessTokenLifespan = 15;
    private static readonly int minutesRefreshTokenLifespan = 60;

    public static Token GenerateToken()
    {
        // Access Token
        byte[] bytes = RandomNumberGenerator.GetBytes(32);
        string accessToken = Convert.ToBase64String(bytes);
        DateTime expirationDateAccessToken = DateTime.UtcNow.AddMinutes(minutesAccessTokenLifespan);

        // Refresh Token
        bytes = RandomNumberGenerator.GetBytes(32);
        string refreshToken = Convert.ToBase64String(bytes);
        DateTime expirationDateRefreshToken = DateTime.UtcNow.AddMinutes(minutesRefreshTokenLifespan);

        _lock.EnterWriteLock();

        try
        {
            token = new Token(accessToken, expirationDateAccessToken, refreshToken, expirationDateRefreshToken);
        }
        finally
        {
            _lock.ExitWriteLock();
        }

        return token;
    }

    /*
    public static bool ValidateToken(
        string token,
        out string? username)
    {
        username = null;

        _lock.EnterReadLock();

        try
        {
            if (!_tokens.TryGetValue(token, out var tokenInfo))
                return false;

            if (tokenInfo.ExpirationDate < DateTime.UtcNow)
                return false;

            username = tokenInfo.Username;
            return true;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }*/

    public static Token? GetToken()
    {
        _lock.EnterReadLock();

        try
        {
            return token;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public static void RevokeToken()
    {
        _lock.EnterWriteLock();

        try
        {
            token = null;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}