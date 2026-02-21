using Queries;

namespace PacketHandlers {

    public abstract class SendingPacket {
        public int status_code;

    }


    public class PacketExtracted  {

        public string? token {get;}
        public IDictionary<string,object>? body {get;}
        public QueriesRequest? queries {get;}

        public PacketExtracted(string? token, IDictionary<string,object>? body, QueriesRequest? queries) {
            this.token = token;
            this.body = body;
            this.queries = queries;
        }

    }

    public class PacketSuccess : SendingPacket {

        public object? body;

        public PacketSuccess(int status_code) {
            this.body = null;
            this.status_code = status_code;
        }

        public PacketSuccess(int status_code, object response) {
            this.body = response;
            this.status_code = status_code;
        }

    }

    public class PacketFail : SendingPacket {

        public string? error_message;
        public IDictionary<string,object>? extra_message = null;

        public PacketFail(int status_code, string error_message, IDictionary<string,object>? extra_message) {
            this.status_code = status_code;
            this.error_message = error_message;
            this.extra_message = extra_message;
        }

        public PacketFail(int status_code,string error_message) {
            this.status_code = status_code;
            this.error_message = error_message;
            this.extra_message = null;
        }

        public PacketFail(int status_code) {
            this.status_code = status_code;
            this.error_message = null;
            this.extra_message = null;
        }

    }


}