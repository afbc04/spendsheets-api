using System.Security.Cryptography;

public class Token {

    public string token {private set; get;}
    public DateTime _expiration_time {private set; get;}

    public Token(int minutes) {

        this.token = _GenerateToken();
        this._expiration_time = DateTime.UtcNow.AddMinutes(minutes);

    }

    public AccessToken GetAccessToken(bool is_writer) {
        return new AccessToken(is_writer,this.IsTokenExpired());
    }

    public static bool IsValid(Token? token) {
        return token != null && token.IsTokenExpired();
    }

    public bool IsTokenExpired() {
        return DateTime.UtcNow <= this._expiration_time;
    }

    private static string _GenerateToken() {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(24));
    }



}