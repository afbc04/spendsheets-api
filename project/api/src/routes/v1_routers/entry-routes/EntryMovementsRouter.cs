using PacketHandlers;

namespace Routers;

public static class EntryMovementsRouters {

    public static RouteGroupBuilder EntryMovementsRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/{entryID}/movements").AllowAnonymous();
        var api = API.GetAPI();

        // GET /v1.0/entries/:entryID/movements
        app.MapGet("", async (HttpRequest request, string entryID) => {

            return await PacketUtils.validate_and_reply(request, "entry-movements/list", async (packet) => {
                return PacketUtils.send_packet(await api.EntryMovements.List(packet.token!,entryID,packet.queries));
            });

        });

        // POST /v1.0/entries/:entryID/movements
        app.MapPost("", async (HttpRequest request, string entryID) => {

            return await PacketUtils.validate_and_reply(request, "entry-movements/create", async (packet) => {
                return PacketUtils.send_packet(await api.EntryMovements.Create(packet.token!,packet.body!,entryID));
            });

        });

        // DELETE /v1.0/entries/:entryID/movements
        app.MapDelete("", async (HttpRequest request, string entryID) => {

            return await PacketUtils.validate_and_reply(request, "entry-movements/clear", async (packet) => {
                return PacketUtils.send_packet(await api.EntryMovements.Clear(packet.token!,entryID,packet.queries));
            });

        });

        // GET /v1.0/entries/:entryID/movements/:noteID
        app.MapGet("{noteID}", async (HttpRequest request, string entryID, string noteID) => {

            return await PacketUtils.validate_and_reply(request, "entry-movements/get", async (packet) => {
                return PacketUtils.send_packet(await api.EntryMovements.Get(packet.token!,entryID,noteID));
            });

        });

        // DELETE /v1.0/entries/:entryID/movements/:noteID
        app.MapDelete("{noteID}", async (HttpRequest request, string entryID, string noteID) => {

            return await PacketUtils.validate_and_reply(request, "entry-movements/delete", async (packet) => {
                return PacketUtils.send_packet(await api.EntryMovements.Delete(packet.token!,entryID,noteID));
            });

        });

        // PUT /v1.0/entries/:entryID/movements/:noteID
        app.MapPut("{noteID}", async (HttpRequest request, string entryID, string noteID) => {

            return await PacketUtils.validate_and_reply(request, "entry-movements/update-full", async (packet) => {
                return PacketUtils.send_packet(await api.EntryMovements.Put(packet.token!,packet.body!,entryID,noteID));
            });

        });

        // PATCH /v1.0/entries/:entryID/movements/:noteID
        app.MapPatch("{noteID}", async (HttpRequest request, string entryID, string noteID) => {

            return await PacketUtils.validate_and_reply(request, "entry-movements/update-partial", async (packet) => {
                return PacketUtils.send_packet(await api.EntryMovements.Patch(packet.token!,packet.body!,entryID,noteID));
            });

        });

        return group;

    }

}