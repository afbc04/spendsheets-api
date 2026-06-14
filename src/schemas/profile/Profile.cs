using System.Security.Cryptography;

public class Profile
{
    private string _username;
    private string _name;
    private DateOnly _creation_date;
    private bool _is_admin;
    private DateOnly? _inactive_date;

    private byte[] _password_hash;
    private byte[] _password_salt;

    public string Username
    {
        get => _username;

        set
        {
            if (value.Length < ProfileRules.UsernameLengthMin) 
                throw new SchemaException($"Username is too short (min {ProfileRules.UsernameLengthMin})");

            if (value.Length > ProfileRules.UsernameLengthMax)
                throw new SchemaException($"Username is too long (max {ProfileRules.UsernameLengthMax})");

            _username = value;
        }
    }

    public string Name
    {
        get => _name;

        set
        {
            if (value.Length == 0) 
                throw new SchemaException($"Name can not be empty");

            if (value.Length > ProfileRules.NameLengthMax)
                throw new SchemaException($"Name is too long (max {ProfileRules.NameLengthMax})");

            _name = value;
        }
    }

    public DateOnly CreationDate
    {
        get => _creation_date;
    }

    public bool IsAdmin
    {
        get => _is_admin;
        set => _is_admin = value;
    }

    public bool IsActive
    {
        get => _inactive_date == null;
        set =>_inactive_date = value ? null : DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public DateOnly? InactiveDate
    {
        get => _inactive_date;
    }

    public byte[] PasswordHash
    {
        get => _password_hash;
    }

    public byte[] PasswordSalt
    {
        get => _password_salt;
    }

    public void SetPassword(string password)
    {
        if (password.Length < ProfileRules.PasswordLengthMin)
            throw new SchemaException($"Password is too short (min {ProfileRules.PasswordLengthMin})");

        using var rng = RandomNumberGenerator.Create();

        _password_salt = new byte[16];
        rng.GetBytes(_password_salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            _password_salt,
            100000,
            HashAlgorithmName.SHA256);

        _password_hash = pbkdf2.GetBytes(32);
    }

    public bool VerifyPassword(string password)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            _password_salt,
            100000,
            HashAlgorithmName.SHA256);

        byte[] computedHash = pbkdf2.GetBytes(32);

        return CryptographicOperations.FixedTimeEquals(computedHash,_password_hash);
    }

    public Profile(
        string username,
        string name,
        DateOnly creationDate,
        bool isAdmin,
        DateOnly? inactiveDate,
        byte[] passwordHash,
        byte[] passwordSalt)
    {
        this._username = username;
        this._name = name;
        this._creation_date = creationDate;
        this._is_admin = isAdmin;
        this._inactive_date = inactiveDate;

        this._password_hash = passwordHash;
        this._password_salt = passwordSalt;
    }

    public Profile(string username = "")
    {
        this._username = username;
        this._name = "";
        this._creation_date = DateOnly.FromDateTime(DateTime.UtcNow);
        this._is_admin = false;
        this._inactive_date = null;

        this._password_hash = [];
        this._password_salt = [];
    }
}

public static class ProfileRules 
{
    public static readonly int PasswordLengthMin = 4;
    public static readonly int UsernameLengthMin = 4;
    public static readonly int UsernameLengthMax = 30;
    public static readonly int NameLengthMax = 40;
}