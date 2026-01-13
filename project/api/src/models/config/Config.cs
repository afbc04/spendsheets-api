using System.Security.Cryptography;

public class Config {

    public int database_version {set; get;}
    public DateTime last_online_date {set; get;}

    public string username {set; get;}

    public byte[] _password {private set; get;}
    public byte[] _salt {private set; get;}

    public string? name {set; get;}
    public bool is_visible_to_public {set; get;}
    public long initial_money {set; get;}
    public long lost_money {set; get;}
    public long saved_money {set; get;}

    // Constructors
    public Config(string username, byte[] password) {

        this.database_version = DAO.DAOManager.database_version;
        this.last_online_date = DateTime.UtcNow;

        this.username = username;
        this._password = password;
        this._salt = [];

        this.name = null;
        this.is_visible_to_public = false;
        this.initial_money = 0;
        this.lost_money = 0;
        this.saved_money = 0;

    }

    public Config(int database_version, DateTime last_online_date, string username, byte[] password, byte[] salt, string? name, bool is_visible_to_public, long initial_money, long lost_money, long saved_money) {

        this.database_version = database_version;
        this.last_online_date = last_online_date;

        this.username = username;
        this._password = password;
        this._salt = salt;

        this.name = name;
        this.is_visible_to_public = is_visible_to_public;
        this.initial_money = initial_money;
        this.lost_money = lost_money;
        this.saved_money = saved_money;

    }

    // Methods
    public void set_password(string password) {

        byte[] salt = RandomNumberGenerator.GetBytes(8);
        byte[] hash_bytes = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256).GetBytes(32);

        this._salt = salt;
        this._password = hash_bytes;

    }

    public bool verify_password(string password_to_check) {

        byte[] hash_bytes = new Rfc2898DeriveBytes(password_to_check, this._salt, 100000, HashAlgorithmName.SHA256).GetBytes(32);
        return CryptographicOperations.FixedTimeEquals(hash_bytes,this._password);

    }

    public IDictionary<string,object?> to_json() {
        return new Dictionary<string,object?> {
            ["username"] = this.username,
            ["name"] = this.name,
            ["public"] = this.is_visible_to_public,
            ["lastOnlineDate"] = Utils.convert_to_datetime(this.last_online_date),
            ["initialMoney"] = Money.Format(this.initial_money),
            ["lostMoney"] = Money.Format(this.lost_money),
            ["savedMoney"] = Money.Format(this.saved_money)
        };
    }


}