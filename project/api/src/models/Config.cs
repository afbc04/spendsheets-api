using System.Security.Cryptography;

public class Config {

    public static readonly int username_length_min = 3;
    public static readonly int username_length_max = 30;
    public static readonly int password_length_min = 4;
    public static readonly int password_length_max = 64;
    public static readonly int name_length_max = 64;

    public string username {set; get;}
    private string _password;
    private long _salt;
    public string? name {set; get;}
    public bool is_visible_to_public {set; get;}
    public uint initial_money {set; get;}
    public uint lost_money {set; get;}
    public uint saved_money {set; get;}

    public IDictionary<string,object?> to_json() {
        return new Dictionary<string,object?> {
            ["username"] = this.username,
            ["name"] = this.name,
            ["public"] = this.is_visible_to_public,
            ["lastOnlineDate"] = null,
            ["initialMoney"] = Utils.convert_to_money(this.initial_money),
            ["lostMoney"] = Utils.convert_to_money(this.lost_money),
            ["savedMoney"] = Utils.convert_to_money(this.saved_money)
        };
    }

    public Config(string username, string password) {

        this.username = username;
        this._password = password;

        this.name = null;
        this.is_visible_to_public = false;
        this.initial_money = 0;
        this.lost_money = 0;
        this.saved_money = 0;

    }

    public Config(string username, string password, long salt, string? name, bool is_visible_to_public, uint initial_money, uint lost_money, uint saved_money) {

        this.username = username;
        this._password = password;
        this._salt = salt;

        this.name = name;
        this.is_visible_to_public = is_visible_to_public;
        this.initial_money = initial_money;
        this.lost_money = lost_money;
        this.saved_money = saved_money;

    }

    public void set_password(string password) {

        byte[] salt_bytes = new byte[8];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(salt_bytes);

        byte[] hash_bytes = new Rfc2898DeriveBytes(password, salt_bytes, 100_000, HashAlgorithmName.SHA256).GetBytes(32);

        this._salt = BitConverter.ToInt64(salt_bytes, 0);
        this._password = BitConverter.ToString(hash_bytes);

    }

    public bool verify_password(string password_to_check) {

        byte[] salt_bytes = BitConverter.GetBytes(this._salt);
        byte[] hash_bytes = new Rfc2898DeriveBytes(password_to_check, salt_bytes, 100_000, HashAlgorithmName.SHA256).GetBytes(32);

        string hashHex = BitConverter.ToString(hash_bytes);
        return hashHex == this._password;

    }


}