using PacketHandlers;

namespace Routers;

public static class EntryTagsRouters {

    public static RouteGroupBuilder EntryTagsRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/{entryID}/tags").AllowAnonymous();
        var api = API.GetAPI();

        // GET /v1.0/entries/:entryID/tags
        app.MapGet("", async (HttpRequest request, string entryID) => {

            return await PacketUtils.validate_and_reply(request, "entry-tags/list", async (packet) => {
                return PacketUtils.send_packet(await api.EntryTags.List(packet.token!,entryID,packet.queries));
            });

        });

        // POST /v1.0/entries/:entryID/tags
        app.MapPost("", async (HttpRequest request, string entryID) => {

            return await PacketUtils.validate_and_reply(request, "entry-tags/create", async (packet) => {
                return PacketUtils.send_packet(await api.EntryTags.Batch(packet.token!,packet.body!,entryID));
            });

        });

        // DELETE /v1.0/entries/:entryID/tags
        app.MapDelete("", async (HttpRequest request, string entryID) => {

            return await PacketUtils.validate_and_reply(request, "entry-tags/clear", async (packet) => {
                return PacketUtils.send_packet(await api.EntryTags.Clear(packet.token!,entryID,packet.queries));
            });

        });

        // DELETE /v1.0/entries/:entryID/tags/:tagID
        app.MapDelete("{tagID}", async (HttpRequest request, string entryID, string tagID) => {

            return await PacketUtils.validate_and_reply(request, "entry-tags/delete", async (packet) => {
                return PacketUtils.send_packet(await api.EntryTags.Delete(packet.token!,entryID,tagID));
            });

        });

        // PUT /v1.0/entries/:entryID/tags/:tagID
        app.MapPut("{tagID}", async (HttpRequest request, string entryID, string tagID) => {

            return await PacketUtils.validate_and_reply(request, "entry-tags/put", async (packet) => {
                return PacketUtils.send_packet(await api.EntryTags.Put(packet.token!,entryID,tagID));
            });

        });

        return group;

    }

}