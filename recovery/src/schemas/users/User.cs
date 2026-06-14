using System.Security.Cryptography;

public class User
{
    private string _username;
    private string? _name;
    private DateOnly _creation_date;
    private long _initial_money;

    private byte[] _password_hash;
    private byte[] _password_salt;

    public string Username
    {
        get => _username;

        set
        {
            if (value.Length < UserRules.UsernameLengthMin) 
                throw new SchemaException(ErrorCategory.USER_USERNAME_MIN);

            if (value.Length > UserRules.UsernameLengthMax)
                throw new SchemaException(ErrorCategory.USER_USERNAME_MAX);

            _username = value;
        }
    }

    public string? Name
    {
        get => _name;

        set
        {
            if (value?.Length == 0) 
                throw new SchemaException(ErrorCategory.USER_NAME_EMPTY);

            if (value?.Length > UserRules.NameLengthMax)
                throw new SchemaException(ErrorCategory.USER_NAME_MAX);

            _name = value;
        }
    }

    public long InitialMoney
    {
        get => _initial_money;
        set => _initial_money = value;
    }

    public DateOnly CreationDate
    {
        get => _creation_date;
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
        if (password.Length < UserRules.PasswordLengthMin)
            throw new SchemaException(ErrorCategory.USER_PASSWORD_MIN);

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

    public User(
        string username,
        string? name,
        long initialMoney,
        DateOnly creationDate,
        byte[] passwordHash,
        byte[] passwordSalt)
    {
        this._username = username;
        this._name = name;
        this._initial_money = initialMoney;
        this._creation_date = creationDate;

        this._password_hash = passwordHash;
        this._password_salt = passwordSalt;
    }

    public User()
    {
        this._username = "";
        this._name = null;
        this._initial_money = 0;
        this._creation_date = DateOnly.FromDateTime(DateTime.UtcNow);

        this._password_hash = [];
        this._password_salt = [];
    }
}

public static class UserRules 
{
    public static readonly int PasswordLengthMin = 4;
    public static readonly int UsernameLengthMin = 4;
    public static readonly int UsernameLengthMax = 30;
    public static readonly int NameLengthMax = 40;
}