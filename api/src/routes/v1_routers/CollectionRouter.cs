using PacketHandlers;
using Templates;

namespace Routers;

public static class CollectionRouters {

    public static RouteGroupBuilder CollectionRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/collections").AllowAnonymous();
        var api = API.GetAPI();

        // GET /v1.0/collections
        app.MapGet("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, CollectionTemplate.List(), async (packet) => {
                return PacketUtils.send_packet(await api.Collection.List(packet.token!,packet.queries));
            });

        });

        // POST /v1.0/collections
        app.MapPost("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, CollectionTemplate.Create(), async (packet) => {
                return PacketUtils.send_packet(await api.Collection.Create(packet.token!,packet.body!));
            });

        });

        // DELETE /v1.0/collections
        app.MapDelete("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, CollectionTemplate.Clear(), async (packet) => {
                return PacketUtils.send_packet(await api.Collection.Clear(packet.token!,packet.queries));
            });

        });

        // PATCH /v1.0/collections
        app.MapPatch("", async (HttpRequest request) => {

            return await PacketUtils.ValidateAndReply(request, CollectionTemplate.Map(), async (packet) => {
                return PacketUtils.send_packet(await api.Collection.Map(packet.token!,packet.body!,packet.queries));
            });

        });

        // GET /v1.0/collections/:id
        app.MapGet("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.ValidateAndReply(request, CollectionTemplate.Get(), async (packet) => {
                return PacketUtils.send_packet(await api.Collection.Get(packet.token,id));
            });

        });

        // DELETE /v1.0/collections/:id
        app.MapDelete("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.ValidateAndReply(request, CollectionTemplate.Delete(), async (packet) => {
                return PacketUtils.send_packet(await api.Collection.Delete(packet.token!,id));
            });

        });

        // PATCH /v1.0/collections/:id
        app.MapPatch("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.ValidateAndReply(request, CollectionTemplate.UpdatePartial(), async (packet) => {
                return PacketUtils.send_packet(await api.Collection.Patch(packet.body!,packet.token!,id));
            });

        });

        // PUT /v1.0/collections/:id
        app.MapPut("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.ValidateAndReply(request, CollectionTemplate.UpdateFull(), async (packet) => {
                return PacketUtils.send_packet(await api.Collection.Update(packet.body!,packet.token!,id));
            });

        });

        return group;

    }

}