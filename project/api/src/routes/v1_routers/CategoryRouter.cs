using PacketHandlers;

namespace Routers;

public static class CategoryRouters {

    public static RouteGroupBuilder CategoryRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/categories").AllowAnonymous();
        var api = API.GetAPI();

        // GET /v1.0/categories
        app.MapGet("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "category/list", async (packet) => {
                return PacketUtils.send_packet(await api.Category.List(packet.token!,packet.queries));
            });

        });

        // POST /v1.0/categories
        app.MapPost("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "category/create", async (packet) => {
                return PacketUtils.send_packet(await api.Category.Create(packet.token!,packet.body!));
            });

        });

        // DELETE /v1.0/categories
        app.MapDelete("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "category/clear", async (packet) => {
                return PacketUtils.send_packet(await api.Category.Clear(packet.token!,packet.queries));
            });

        });

        // GET /v1.0/categories/:id
        app.MapGet("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "category/get", async (packet) => {
                return PacketUtils.send_packet(await api.Category.Get(packet.token,id));
            });

        });

        // DELETE /v1.0/categories/:id
        app.MapDelete("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "category/delete", async (packet) => {
                return PacketUtils.send_packet(await api.Category.Delete(packet.token!,id));
            });

        });

        // PATCH /v1.0/categories/:id
        app.MapPatch("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "category/update-partial", async (packet) => {
                return PacketUtils.send_packet(await api.Category.Patch(packet.body!,packet.token!,id));
            });

        });

        // PUT /v1.0/categories/:id
        app.MapPut("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "category/update-full", async (packet) => {
                return PacketUtils.send_packet(await api.Category.Update(packet.body!,packet.token!,id));
            });

        });

        return group;

    }

}