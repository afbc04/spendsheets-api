using PacketHandlers;
using Nito.AsyncEx;

namespace Controller {

    public class TokenController {

        public readonly AsyncReaderWriterLock Lock;
        private volatile Token? token;

        public TokenController() {
            this.Lock = new();
            this.token = null;
        }

        // @@@@@@@@@@@@@@@@@@@@@@@@@
        //  Auxiliar Funcionalities
        // @@@@@@@@@@@@@@@@@@@@@@@@@
        public AccessToken? _GetToken(string? token) {

            if (this.token == null || token == null)
                return null;

            return this.token.token == token ? this.token.get_access_token() : null;

        }

        // @@@@@@@@@@@@@@@@@@@@@@@@@
        //    Main Funcionalities
        // @@@@@@@@@@@@@@@@@@@@@@@@@
        public SendingPacket Get() {

            bool valid = Token.is_valid(this.token);
            bool? writer = valid == true ? this.token!.is_writer : null;

            return new PacketSuccess(valid ? 200 : 401, new Dictionary<string,object?> {
                ["valid"] = valid,
                ["writer"] = writer
            });

        }

        public SendingPacket Create(IDictionary<string,object> token_data, Config config) {

            if (config.username != (string) token_data["username"] || config.verify_password((string) token_data["password"]) == false)
                return new PacketFail(403,"Username or password does not match up to configuration of system");

            this.token = new Token((bool) token_data["writer"]);
            return new PacketSuccess(201,this.token.to_json());

        }

        public SendingPacket Delete(AccessToken? token) {
        
            if (token == null)
                return SendErrors.InvalidToken();
            
            this.token = null;
            return new PacketSuccess(200,new Dictionary<string,object>{
                ["message"] = "Token was deleted"
            });

        }

        public SendingPacket Update(IDictionary<string,object> token_data, AccessToken? token, Config config) {

            if (AccessToken.IsValid(token) == false)
                return SendErrors.InvalidToken(token);

            bool is_writer = (bool) token_data["writer"];

            // Switch to writer mode
            if (is_writer == true) {

                if (token_data.ContainsKey("username") && token_data.ContainsKey("password")) {

                    if (config.username != (string) token_data["username"] || config.verify_password((string) token_data["password"]) == false)
                        return new PacketFail(403,"Username or password does not match up to configuration of system. Could not switch to writer mode!");
                    else
                        this.token!.is_writer = true;

                }
                else
                    return new PacketFail(417,"In order to switch token to writer mode, username and password should be provided");

            }
            // Switch to reader mode
            else {
                this.token!.is_writer = false;
            }

            return new PacketSuccess(200,new Dictionary<string,object?> {
                    ["valid"] = true,
                    ["writer"] = is_writer
                });


        }


    }

}
