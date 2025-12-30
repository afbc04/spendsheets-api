using PacketHandlers;
using Queries;

namespace Routers;

public static class ConfigRouters {

    public static RouteGroupBuilder ConfigRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/config").AllowAnonymous();

        // POST /v1.0/config
        app.MapPost("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "config/create", async (packet) => {
                return PacketUtils.send_packet(await API.controller!.config_create(packet.body!));
            });

        });

        // PUT /v1.0/config
        app.MapPut("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "config/update", async (packet) => {
                return PacketUtils.send_packet(await API.controller!.config_update(packet.body!));
            });

        });

        // GET /v1.0/config
        app.MapGet("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "config/get", async (packet) => {
                return PacketUtils.send_packet(await API.controller!.config_get());
            });

        });

        return group;

    }

}