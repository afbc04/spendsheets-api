using PacketHandlers;
using Queries;

namespace Routers;

public static class ConfigRouters {

    public static RouteGroupBuilder ConfigRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/config").AllowAnonymous();

        var api = API.GetAPI();

        // POST /v1.0/config
        app.MapPost("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "config/create", async (packet) => {
                return PacketUtils.send_packet(await api.Config.Create(packet.body!));
            });

        });

        // PUT /v1.0/config
        app.MapPut("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "config/update", async (packet) => {
                return PacketUtils.send_packet(await api.Config.Update(packet.body!,packet.token));
            });

        });

        // GET /v1.0/config
        app.MapGet("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "config/get", async (packet) => {
                return PacketUtils.send_packet(await api.Config.Get());
            });

        });

        return group;

    }

}