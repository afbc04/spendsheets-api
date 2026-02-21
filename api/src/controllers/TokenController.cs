using PacketHandlers;
using Nito.AsyncEx;

namespace Controller {

    public class TokenController {

        public readonly AsyncReaderWriterLock Lock;
        private volatile Token? writer_token;
        private string? reader_secret;
        private volatile Token? reader_token;
        private bool is_system_public;

        private static readonly int writer_token_minutes_expires_in = 60;
        private static readonly int reader_token_minutes_expires_in = 60;

        public TokenController(bool is_system_public) {
            this.Lock = new();
            this.writer_token = null;
            this.reader_secret = null;
            this.reader_token = null;
            this.is_system_public = is_system_public;
        }

        // @@@@@@@@@@@@@@@@@@@@@@@@@
        //  Auxiliar Funcionalities
        // @@@@@@@@@@@@@@@@@@@@@@@@@
        public AccessToken? _GetToken(string? token) {

            if (token == null)
                return null;

            if (this.writer_token != null && this.writer_token.token == token)
                return this.writer_token.GetAccessToken(true);
            
            else if (this.reader_token != null && this.reader_token.token == token)
                return this.reader_token.GetAccessToken(false);

            else
                return null;

        }

        public void _DeleteWriterToken() {
            this.writer_token = null;
        }

        public void _DeleteReaderToken() {
            this.reader_secret = null;
            this.reader_token = null;
        }

        public void _SetSystemVisibility(bool is_public) {
            this.is_system_public = is_public;
        }

        public bool _IsSystemPublic() {
            return this.is_system_public;
        }

        // @@@@@@@@@@@@@@@@@@@@@@@@@
        //    Main Funcionalities
        // @@@@@@@@@@@@@@@@@@@@@@@@@
        public SendingPacket VerifyToken(string? token) {

            bool is_valid = false;
            bool is_writer = false;

            if (token == null)
                return new PacketFail(404,"No token provided");

            if (this.writer_token != null && this.writer_token.token == token) {
                is_valid = Token.IsValid(this.writer_token);
                is_writer = true;
            }
            
            else if (this.reader_token != null && this.reader_token.token == token) {
                is_valid = Token.IsValid(this.reader_token);
                is_writer = false;
            }

            return is_valid == false ? new PacketFail(401,"Token provided is not valid") : 
                new PacketSuccess(200,new Dictionary<string,object>() {
                    ["info"] = "Token provided is valid",
                    ["writer"] = is_writer
                });

        }

        public SendingPacket ObtainToken(IDictionary<string,object> token_data) {

            bool is_writer = (bool) token_data["writer"];
            string secret_provided = (string) token_data["secret"];

            return is_writer ? _ObtainWriterToken(secret_provided) : _ObtainReaderToken(secret_provided);

        }

        private SendingPacket _ObtainWriterToken(string secret_provided) {

            string? secret = Environment.GetEnvironmentVariable("USER_SECRET");

            if (secret == null)
                return new PacketFail(500,"Secret was not set up. System cannot provide access tokens");


            if (secret != secret_provided)
                return new PacketFail(403,"Secret provided does not match up to configuration of system");

            this.writer_token = new Token(TokenController.writer_token_minutes_expires_in);
            return new PacketSuccess(201,TokenResponse.ToJson(this.writer_token,true));

        }

        private SendingPacket _ObtainReaderToken(string secret_provided) {

            if (this.reader_token == null)
                return new PacketFail(403,"Administrator did not set up reader access tokens. You cannot obtain it");

            if (this.reader_secret == null)
                return new PacketFail(403,"Administrator did not set up secret for readers. You cannot obtain reader access token");

            if (this.reader_secret != secret_provided)
                return new PacketFail(403,"Secret does not match up the reader secret");

            return new PacketSuccess(200,TokenResponse.ToJson(this.reader_token,false));

        }

        public SendingPacket CreateReader(IDictionary<string,object> token_data, string token_extracted) {

            var token = _GetToken(token_extracted);

            if (AccessToken.IsValid(token) == false || (token != null && token.is_writer == false))
                return SendErrors.WriterTokenNeeded();

            Console.WriteLine(token);
            Console.WriteLine(AccessToken.IsValid(token) == false);
            Console.WriteLine(token != null && token.is_writer == false);

            this.reader_secret = (string) token_data["secretReader"];
            int reader_token_minutes_expired_in = token_data.ContainsKey("expiresIn") ? Convert.ToInt32(token_data["expiresIn"]) : TokenController.reader_token_minutes_expires_in;

            if (reader_token_minutes_expired_in <= 0)
                return new PacketSuccess(417,"Expiration time must be positive");

            this.reader_token = new Token(reader_token_minutes_expired_in);
            return new PacketSuccess(201,new Dictionary<string,object>() {
                ["info"] = "Reader token has been set up"
            });

        }

        public SendingPacket InvalidateToken(IDictionary<string,object> token_data, string token_extracted) {
        
            var token = _GetToken(token_extracted);

            if (token == null)
                return SendErrors.InvalidToken();
            
            if ((bool) token_data["writer"])
                _DeleteWriterToken();
            else
                _DeleteReaderToken();

            string message = token.is_writer ? "Writer token was deleted" : "Reader token was deleted";
                
            return new PacketSuccess(200,new Dictionary<string,object>{
                ["message"] = message
            });

        }

    }

}