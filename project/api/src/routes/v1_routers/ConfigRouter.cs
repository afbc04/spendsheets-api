using PacketHandlers;
using Queries;

namespace Routers;

public static class ConfigRouters {

    public static RouteGroupBuilder ConfigRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/config").AllowAnonymous();

        var api = API.GetAPI();

        // GET /v1.0/config
        app.MapGet("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "config/get", async (packet) => {
                return PacketUtils.send_packet(await api.Config.Get());
            });

        });

        // POST /v1.0/config
        app.MapPost("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "config/create", async (packet) => {
                return PacketUtils.send_packet(await api.Config.Create(packet.body!));
            });

        });

        // PATCH /v1.0/config
        app.MapPatch("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "config/update-partial", async (packet) => {
                return PacketUtils.send_packet(await api.Config.Patch(packet.body!,packet.token));
            });

        });

        // PUT /v1.0/config
        app.MapPut("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "config/update-full", async (packet) => {
                return PacketUtils.send_packet(await api.Config.Update(packet.body!,packet.token));
            });

        });

        // DELETE /v1.0/config
        app.MapDelete("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "config/delete", async (packet) => {
                return PacketUtils.send_packet(await api.Config.Delete(packet.token));
            });

        });

        return group;

    }

}