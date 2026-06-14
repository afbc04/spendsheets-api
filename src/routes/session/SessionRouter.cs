public static class SessionRouters {

    public static RouteGroupBuilder SessionRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/session").AllowAnonymous();

        // GET /session
        app.MapGet("", async (HttpRequest request) => {

            var session = await ValidatorSessionMiddleware.TryValidateSession(request);
            string token = session?.Token ?? "";
            var resultPacket = await SessionController.GetSession(token ?? "");
            return resultPacket.Send();

        });

        // POST /session
        app.MapPost("", async (HttpRequest request) => {

            var body = await ValidatorBodyMiddleware.ValidateBody(request, SessionBodyValidatorsTemplate.Obtain());
            var resultPacket = await SessionController.ObtainSession(body!);
            return resultPacket.Send();

        });

        // DELETE /session
        app.MapDelete("", async (HttpRequest request) => {

            var session = await ValidatorSessionMiddleware.ValidateSession(request);
            var resultPacket = await SessionController.RevokeSession(session.Token!);
            return resultPacket.Send();

        });

        return group;
    }
}