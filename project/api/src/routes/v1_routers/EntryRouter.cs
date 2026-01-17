using PacketHandlers;

namespace Routers;

public static class EntryRouters {

    public static RouteGroupBuilder EntryRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/entries").AllowAnonymous();
        var api = API.GetAPI();

        // GET /v1.0/entries
        app.MapGet("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "monthly-service/list", async (packet) => {
                return PacketUtils.send_packet(await api.Entry.List(packet.token!,packet.queries));
            });

        });

        // POST /v1.0/entries
        app.MapPost("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "entry/create", async (packet) => {
                return PacketUtils.send_packet(await api.Entry.Create(packet.token!,packet.body!));
            });

        });

        /*
        // DELETE /v1.0/monthlyServices
        app.MapDelete("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "monthly-service/clear", async (packet) => {
                return PacketUtils.send_packet(await api.MonthlyService.Clear(packet.token!,packet.queries));
            });

        });

        // PATCH /v1.0/monthlyServices
        app.MapPatch("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "monthly-service/update-all", async (packet) => {
                return PacketUtils.send_packet(await api.MonthlyService.Map(packet.token!,packet.body!,packet.queries));
            });

        });*/

        // GET /v1.0/entries/:id
        app.MapGet("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "entry/get", async (packet) => {
                return PacketUtils.send_packet(await api.Entry.Get(packet.token,id));
            });

        });

        // DELETE /v1.0/entries/:id
        app.MapDelete("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "entry/delete", async (packet) => {
                return PacketUtils.send_packet(await api.Entry.Delete(packet.token!,id));
            });

        });

        // PATCH /v1.0/entries/:id
        app.MapPatch("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "entry/update-partial", async (packet) => {
                return PacketUtils.send_packet(await api.Entry.Patch(packet.body!,packet.token!,id));
            });

        });

        // PUT /v1.0/entries/:id
        app.MapPut("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "entry/update-full", async (packet) => {
                return PacketUtils.send_packet(await api.Entry.Update(packet.body!,packet.token!,id));
            });

        });

        app.EntryTagsRoutersMapping();
        app.EntryNotesRoutersMapping();
        app.EntryMovementsRoutersMapping();

        return group;

    }

}