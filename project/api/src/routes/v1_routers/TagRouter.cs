using PacketHandlers;

namespace Routers;

public static class TagRouters {

    public static RouteGroupBuilder TagRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/tags").AllowAnonymous();
        var api = API.GetAPI();

        // GET /v1.0/tags
        app.MapGet("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "tag/list", async (packet) => {
                return PacketUtils.send_packet(await api.Tag.List(packet.token!,packet.queries));
            });

        });

        // POST /v1.0/tags
        app.MapPost("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "tag/create", async (packet) => {
                return PacketUtils.send_packet(await api.Tag.Create(packet.token!,packet.body!));
            });

        });

        // DELETE /v1.0/tags
        app.MapDelete("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "tag/clear", async (packet) => {
                return PacketUtils.send_packet(await api.Tag.Clear(packet.token!,packet.queries));
            });

        });

        // GET /v1.0/tags/:id
        app.MapGet("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "tag/get", async (packet) => {
                return PacketUtils.send_packet(await api.Tag.Get(packet.token,id));
            });

        });

        // DELETE /v1.0/tags/:id
        app.MapDelete("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "tag/delete", async (packet) => {
                return PacketUtils.send_packet(await api.Tag.Delete(packet.token!,id));
            });

        });

        // PATCH /v1.0/tags/:id
        app.MapPatch("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "tag/update-partial", async (packet) => {
                return PacketUtils.send_packet(await api.Tag.Patch(packet.body!,packet.token!,id));
            });

        });

        // PUT /v1.0/tags/:id
        app.MapPut("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "tag/update-full", async (packet) => {
                return PacketUtils.send_packet(await api.Tag.Update(packet.body!,packet.token!,id));
            });

        });

        return group;

    }

}