using PacketHandlers;

namespace Routers;

public static class EntryTagsRouters {

    public static RouteGroupBuilder EntryTagsRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/entryTags").AllowAnonymous();
        var api = API.GetAPI();

        // GET /v1.0/entryTags/:entryID
        app.MapGet("{entryID}", async (HttpRequest request, string entryID) => {

            return await PacketUtils.validate_and_reply(request, "entry-tags/list", async (packet) => {
                return PacketUtils.send_packet(await api.EntryTags.List(packet.token!,entryID,packet.queries));
            });

        });

        /*
        // POST /v1.0/entries
        app.MapPost("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "entry/create", async (packet) => {
                return PacketUtils.send_packet(await api.Entry.Create(packet.token!,packet.body!));
            });

        });*/

        // DELETE /v1.0/entryTags/:entryID
        app.MapDelete("{entryID}", async (HttpRequest request, string entryID) => {

            return await PacketUtils.validate_and_reply(request, "entry-tags/clear", async (packet) => {
                return PacketUtils.send_packet(await api.EntryTags.Clear(packet.token!,entryID));
            });

        });

        // GET /v1.0/entryTags/:entryID/:tagID
        app.MapGet("{entryID}/{tagID}", async (HttpRequest request, string entryID, string tagID) => {

            return await PacketUtils.validate_and_reply(request, "entry-tags/get", async (packet) => {
                return PacketUtils.send_packet(await api.EntryTags.Get(packet.token,entryID,tagID));
            });

        });

        // DELETE /v1.0/entryTags/:entryID/:tagID
        app.MapDelete("{entryID}/{tagID}", async (HttpRequest request, string entryID, string tagID) => {

            return await PacketUtils.validate_and_reply(request, "entry-tags/delete", async (packet) => {
                return PacketUtils.send_packet(await api.EntryTags.Delete(packet.token!,entryID,tagID));
            });

        });

        // PUT /v1.0/entryTags/:entryID/:tagID
        app.MapPut("{entryID}/{tagID}", async (HttpRequest request, string entryID, string tagID) => {

            return await PacketUtils.validate_and_reply(request, "entry-tags/put", async (packet) => {
                return PacketUtils.send_packet(await api.EntryTags.Put(packet.token!,entryID,tagID));
            });

        });

        return group;

    }

}