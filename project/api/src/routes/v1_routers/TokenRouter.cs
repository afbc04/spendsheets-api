using PacketHandlers;
using Queries;

namespace Routers;

public static class TokenRouters {

    public static RouteGroupBuilder TokenRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/token").AllowAnonymous();
        var api = API.GetAPI();

        // GET /v1.0/token
        app.MapGet("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "token/get", async (packet) => {
                return PacketUtils.send_packet(await api.Token.Get());
            });

        });

        // POST /v1.0/token
        app.MapPost("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "token/create", async (packet) => {
                return PacketUtils.send_packet(await api.Token.Create(packet.body!));
            });

        });

        // DELETE /v1.0/token
        app.MapDelete("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "token/delete", async (packet) => {
                return PacketUtils.send_packet(await api.Token.Delete(packet.token!));
            });

        });

        // PATCH /v1.0/token
        app.MapPatch("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "token/patch", async (packet) => {
                return PacketUtils.send_packet(await api.Token.Patch(packet.body!,packet.token!));
            });

        });

        return group;

    }

}