using PacketHandlers;

namespace Routers;

public static class MonthlyServiceRouters {

    public static RouteGroupBuilder MonthlyServiceRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/monthlyServices").AllowAnonymous();
        var api = API.GetAPI();

        // GET /v1.0/monthlyServices
        app.MapGet("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "monthly-service/list", async (packet) => {
                return PacketUtils.send_packet(await api.MonthlyService.List(packet.token!,packet.queries));
            });

        });

        // POST /v1.0/monthlyServices
        app.MapPost("", async (HttpRequest request) => {

            return await PacketUtils.validate_and_reply(request, "monthly-service/create", async (packet) => {
                return PacketUtils.send_packet(await api.MonthlyService.Create(packet.token!,packet.body!));
            });

        });

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

        });

        // GET /v1.0/monthlyServices/:id
        app.MapGet("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "monthly-service/get", async (packet) => {
                return PacketUtils.send_packet(await api.MonthlyService.Get(packet.token,id));
            });

        });

        // DELETE /v1.0/monthlyServices/:id
        app.MapDelete("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "monthly-service/delete", async (packet) => {
                return PacketUtils.send_packet(await api.MonthlyService.Delete(packet.token!,id));
            });

        });

        // PATCH /v1.0/monthlyServices/:id
        app.MapPatch("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "monthly-service/update-partial", async (packet) => {
                return PacketUtils.send_packet(await api.MonthlyService.Patch(packet.body!,packet.token!,id));
            });

        });

        // PUT /v1.0/monthlyServices/:id
        app.MapPut("{id}", async (HttpRequest request, string id) => {

            return await PacketUtils.validate_and_reply(request, "monthly-service/update-full", async (packet) => {
                return PacketUtils.send_packet(await api.MonthlyService.Update(packet.body!,packet.token!,id));
            });

        });

        return group;

    }

}