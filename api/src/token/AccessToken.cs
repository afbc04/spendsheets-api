public class AccessToken {

    public bool is_writer {private set; get;}
    public bool is_valid {private set; get;}

    public AccessToken(bool is_writer, bool is_valid) {

        this.is_writer = is_writer;
        this.is_valid = is_valid;

    }

    public static bool IsValid(AccessToken? token) {
        return token != null && token.is_valid == true;
    }

    public static bool CanRead(AccessToken? token) {
        return token != null;
    }

}