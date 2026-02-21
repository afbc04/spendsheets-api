using PacketHandlers;
using Controller;

public class TokenManager {

    private TokenController token;

    public TokenManager(TokenController token) {
        this.token = token;
    }

    public async Task<SendingPacket> VerifyToken(string token_extracted) {

        using (await token.Lock.ReaderLockAsync())
            return this.token.VerifyToken(token_extracted);

    }

    public async Task<SendingPacket> ObtainToken(IDictionary<string,object> request_data) {

        using (await token.Lock.WriterLockAsync())
            return this.token.ObtainToken(request_data);

    }

    public async Task<SendingPacket> CreateReaderToken(IDictionary<string,object> request_data, string token_extracted) {

        using (await token.Lock.WriterLockAsync())
            return this.token.CreateReader(request_data,token_extracted);

    }

    public async Task<SendingPacket> InvalidateToken(IDictionary<string,object> request_data, string token_extracted) {

        using (await token.Lock.WriterLockAsync())
            return this.token.InvalidateToken(request_data,token_extracted);

    }

}