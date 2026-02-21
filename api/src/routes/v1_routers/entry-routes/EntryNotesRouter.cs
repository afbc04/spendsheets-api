using PacketHandlers;
using Templates;

namespace Routers;

public static class EntryNotesRouters {

    public static RouteGroupBuilder EntryNotesRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/{entryID}/notes").AllowAnonymous();
        var api = API.GetAPI();

        // GET /v1.0/entries/:entryID/notes
        app.MapGet("", async (HttpRequest request, string entryID) => {

            return await PacketUtils.ValidateAndReply(request, EntryNoteTemplate.List(), async (packet) => {
                return PacketUtils.send_packet(await api.EntryNotes.List(packet.token!,entryID,packet.queries));
            });

        });

        // POST /v1.0/entries/:entryID/notes
        app.MapPost("", async (HttpRequest request, string entryID) => {

            return await PacketUtils.ValidateAndReply(request, EntryNoteTemplate.Create(), async (packet) => {
                return PacketUtils.send_packet(await api.EntryNotes.Create(packet.token!,packet.body!,entryID));
            });

        });

        /*
        // DELETE /v1.0/entries/:entryID/notes
        app.MapDelete("", async (HttpRequest request, string entryID) => {

            return await PacketUtils.validate_and_reply(request, "entry-notes/clear", async (packet) => {
                return PacketUtils.send_packet(await api.EntryNotes.Clear(packet.token!,entryID,packet.queries));
            });

        });*/

        // GET /v1.0/entries/:entryID/notes/:noteID
        app.MapGet("{noteID}", async (HttpRequest request, string entryID, string noteID) => {

            return await PacketUtils.ValidateAndReply(request, EntryNoteTemplate.Get(), async (packet) => {
                return PacketUtils.send_packet(await api.EntryNotes.Get(packet.token!,entryID,noteID));
            });

        });

        /*
        // DELETE /v1.0/entries/:entryID/notes/:noteID
        app.MapDelete("{noteID}", async (HttpRequest request, string entryID, string noteID) => {

            return await PacketUtils.validate_and_reply(request, "entry-notes/delete", async (packet) => {
                return PacketUtils.send_packet(await api.EntryNotes.Delete(packet.token!,entryID,noteID));
            });

        });

        // PATCH /v1.0/entries/:entryID/notes/:noteID
        app.MapPatch("{noteID}", async (HttpRequest request, string entryID, string noteID) => {

            return await PacketUtils.validate_and_reply(request, "entry-notes/update-partial", async (packet) => {
                return PacketUtils.send_packet(await api.EntryNotes.Patch(packet.token!,packet.body!,entryID,noteID));
            });

        });

        // PUT /v1.0/entries/:entryID/notes/:noteID
        app.MapPut("{noteID}", async (HttpRequest request, string entryID, string noteID) => {

            return await PacketUtils.validate_and_reply(request, "entry-notes/update-full", async (packet) => {
                return PacketUtils.send_packet(await api.EntryNotes.Update(packet.token!,packet.body!,entryID,noteID));
            });

        });*/

        return group;

    }

}