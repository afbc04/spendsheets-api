public class Session
{
    public static readonly double ExpirationMinutes = 20;

    private Profile _profile;
    private DateTime? _last_updated_session;
    public string? Token {set; get;}

    public Session(Profile profile)
    {
        this._profile = profile;
        this._last_updated_session = null;
        this.Token = null;
    }

    public Profile Profile { get => _profile; set => _profile = value;}
    public DateTime? LastUpdated { get => _last_updated_session;}

    public bool IsValid()
    {
        return Token != null && DateTime.UtcNow < _last_updated_session?.AddMinutes(ExpirationMinutes) && _profile.IsActive;
    }

    public void Refresh()
    {
        _last_updated_session = DateTime.UtcNow;
    }
}