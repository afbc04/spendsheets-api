using PacketHandlers;
using Templates;

namespace Routers;

public static class TokenRouters {

    public static RouteGroupBuilder TokenRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/token").AllowAnonymous();
        var api = API.GetAPI();

        // GET /v1.0/token
        app.MapGet("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, TokenTemplate.ValidateToken(), async (packet) => {
                return PacketUtils.send_packet(await api.Token.VerifyToken(packet.token!));
            });

        });

        // POST /v1.0/token
        app.MapPost("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, TokenTemplate.ObtainToken(), async (packet) => {
                return PacketUtils.send_packet(await api.Token.ObtainToken(packet.body!));
            });

        });

        // DELETE /v1.0/token
        app.MapDelete("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, TokenTemplate.InvalidateToken(), async (packet) => {
                return PacketUtils.send_packet(await api.Token.InvalidateToken(packet.body!,packet.token!));
            });

        });

        return group;

    }

    public static RouteGroupBuilder ReaderTokenRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/readerToken").AllowAnonymous();
        var api = API.GetAPI();

        // POST /v1.0/readerToken
        app.MapPost("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, TokenTemplate.SetReaderToken(), async (packet) => {
                return PacketUtils.send_packet(await api.Token.CreateReaderToken(packet.body!,packet.token!));
            });

        });

        return group;

    }

}