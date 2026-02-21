using PacketHandlers;
using Templates;
namespace Routers;

public static class CategoryRouters {

    public static RouteGroupBuilder CategoryRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/categories").AllowAnonymous();
        var api = API.GetAPI();

        // GET /v1.0/categories
        app.MapGet("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, CategoryTemplate.List(), async (packet) => {
                return PacketUtils.send_packet(await api.Category.List(packet.token!,packet.queries));
            });

        });

        // POST /v1.0/categories
        app.MapPost("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, CategoryTemplate.Create(), async (packet) => {
                return PacketUtils.send_packet(await api.Category.Create(packet.token!,packet.body!));
            });

        });

        // DELETE /v1.0/categories
        app.MapDelete("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, CategoryTemplate.Clear(), async (packet) => {
                return PacketUtils.send_packet(await api.Category.Clear(packet.token!,packet.queries));
            });

        });

        // GET /v1.0/categories/:id
        app.MapGet("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.ValidateAndReply(request, CategoryTemplate.Get(), async (packet) => {
                return PacketUtils.send_packet(await api.Category.Get(packet.token,id));
            });

        });

        // DELETE /v1.0/categories/:id
        app.MapDelete("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.ValidateAndReply(request, CategoryTemplate.Delete(), async (packet) => {
                return PacketUtils.send_packet(await api.Category.Delete(packet.token!,id));
            });

        });

        // PATCH /v1.0/categories/:id
        app.MapPatch("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.ValidateAndReply(request, CategoryTemplate.UpdatePartial(), async (packet) => {
                return PacketUtils.send_packet(await api.Category.Patch(packet.body!,packet.token!,id));
            });

        });

        // PUT /v1.0/categories/:id
        app.MapPut("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.ValidateAndReply(request, CategoryTemplate.UpdateFull(), async (packet) => {
                return PacketUtils.send_packet(await api.Category.Update(packet.body!,packet.token!,id));
            });

        });

        return group;

    }

}