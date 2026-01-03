using System.Security.Cryptography;

public class Token {

    public bool is_writer {set; get;}
    public string token {private set; get;}
    public DateTime _expiration_time {private set; get;}
    private static readonly int minutes_until_expires = 15;

    public IDictionary<string,object?> to_json_details() {
        return new Dictionary<string,object?> {
            ["valid"] = this.is_token_expired(),
            ["writer"] = this.is_writer
        };
    }

    public IDictionary<string,object?> to_json() {
        return new Dictionary<string,object?> {
            ["accessToken"] = this.token,
            ["tokenType"] = "Bearer",
            ["expiresIn"] = Token.minutes_until_expires,
            ["valid"] = true,
            ["writer"] = true
        };
    }

    public Token(bool is_writer) {

        this.token = _generate_token();
        this.is_writer = is_writer;
        this._expiration_time = DateTime.UtcNow.AddMinutes(Token.minutes_until_expires);

    }

    public AccessToken get_access_token() {
        return new AccessToken(this.is_writer,this.is_token_expired());
    }

    public static bool is_valid(Token? token) {
        return token != null && token.is_token_expired();
    }

    public bool is_token_expired() {
        return DateTime.UtcNow <= this._expiration_time;
    }

    private static string _generate_token() {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(24));
    }



}