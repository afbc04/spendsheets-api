using System.Security.Cryptography;

public class SessionManager
{
    private static SessionManager? _singleton = null;

    private readonly ReaderWriterLockSlim _lock = new();

    private readonly Dictionary<string, Session> _sessions = [];
    private readonly Dictionary<string, string> _tokens = [];

    private SessionManager(List<Profile> profiles)
    {
        foreach (Profile p in profiles)
            _sessions[p.Username] = new Session(p);
    }

    public static async Task<bool> InitSessionManager()
    {
        var profiles = await ProfileRepository.List();
        _singleton = new SessionManager(profiles);
        return true;
    }

    /* ==========================
                SESSIONS
       ==========================*/
    public static Session? GetSession(string token)
    {
        _singleton!._lock.EnterReadLock();
        try
        {
            if (_singleton!._tokens.TryGetValue(token, out var username))
                return _singleton!._sessions[username];

            return null;
        }
        finally
        {
            _singleton!._lock.ExitReadLock();
        }
    }

    public static Session? CreateSession(string username)
    {
        _singleton!._lock.EnterWriteLock();
        try
        {
            if (_singleton!._sessions.TryGetValue(username, out var session))
            {
                string? oldToken = session.Token;
                if (oldToken is not null)
                    _singleton!._tokens.Remove(oldToken);

                byte[] bytes = RandomNumberGenerator.GetBytes(32);
                string newToken = Convert.ToBase64String(bytes);
                _singleton!._tokens[newToken] = username;
                session.Token = newToken;
                session.Refresh();

                return session;
            }

            return null;
        }
        finally
        {
            _singleton!._lock.ExitWriteLock();
        }
    }

    public static bool RevokeSession(string token)
    {
        _singleton!._lock.EnterWriteLock();
        try
        {
            if (_singleton!._tokens.TryGetValue(token, out var username))
            {
                var session = _singleton!._sessions[username];
                session.Token = null;
                _singleton!._tokens.Remove(token);
                return true;
            }

            return false;
        }
        finally
        {
            _singleton!._lock.ExitWriteLock();
        }
    }

    /* ==========================
                PROFILE
       ==========================*/
    public static Session? GetProfile(string username)
    {
        _singleton!._lock.EnterReadLock();
        try
        {
            return _singleton!._sessions.TryGetValue(username, out var session) ? session : null;
        }
        finally
        {
            _singleton!._lock.ExitReadLock();
        }
    }

    public static Session? PutProfile(Profile profile)
    {
        _singleton!._lock.EnterWriteLock();
        try
        {
            if (_singleton!._sessions.TryGetValue(profile.Username, out var session))
            {
                session.Profile = profile;
                return session;
            }
            else
            {
                var newSession = new Session(profile);
                _singleton!._sessions[profile.Username] = newSession;
                return newSession;
            }
        }
        finally
        {
            _singleton!._lock.ExitWriteLock();
        }
    }

    public static Session? DeleteProfile(string username)
    {
        _singleton!._lock.EnterWriteLock();
        try
        {
            if (_singleton!._sessions.TryGetValue(username, out var session))
            {
                _singleton!._sessions.Remove(username);
                if (session.Token is not null) _singleton!._tokens.Remove(session.Token);
                return session;
            }
            else
                return null;
        }
        finally
        {
            _singleton!._lock.ExitWriteLock();
        }
    }

    public static List<Session> ListSessions()
    {
        _singleton!._lock.EnterReadLock();
        try
        {
            return [.. _singleton!._sessions.Values];
        }
        finally
        {
            _singleton!._lock.ExitReadLock();
        }
    }

    public static long CountProfiles()
    {
        _singleton!._lock.EnterReadLock();
        try
        {
            return _singleton!._sessions.Count;
        }
        finally
        {
            _singleton!._lock.ExitReadLock();
        }
    }
}