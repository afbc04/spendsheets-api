using PacketHandlers;
using Queries;
using Templates;

namespace Routers;

public static class ConfigRouters {

    public static RouteGroupBuilder ConfigRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/config").AllowAnonymous();

        var api = API.GetAPI();

        // GET /v1.0/config
        app.MapGet("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, ConfigTemplate.Get(), async (packet) => {
                return PacketUtils.send_packet(await api.Config.Get());
            });

        });

        // PATCH /v1.0/config
        app.MapPatch("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, ConfigTemplate.Patch(), async (packet) => {
                return PacketUtils.send_packet(await api.Config.Patch(packet.body!,packet.token));
            });

        });

        // PUT /v1.0/config
        app.MapPut("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, ConfigTemplate.Put(), async (packet) => {
                return PacketUtils.send_packet(await api.Config.Update(packet.body!,packet.token));
            });

        });

        return group;

    }

}