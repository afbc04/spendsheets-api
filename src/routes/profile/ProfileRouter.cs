public static class ProfileRouters {

    public static RouteGroupBuilder ProfileRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/profiles").AllowAnonymous();

        // GET /profiles
        app.MapGet("", async (HttpRequest request) => {

            await ValidatorSessionMiddleware.TryValidateSession(request);
            var resultPacket = await ProfileController.ListProfiles(false);
            return resultPacket.Send();

        });

        // POST /v1.0/user
        app.MapPost("", async (HttpRequest request) => {

            var session = await ValidatorSessionMiddleware.TryValidateSession(request);
            var body = await ValidatorBodyMiddleware.ValidateBody(request, ProfileBodyValidatorsTemplate.Create());
            var resultPacket = await ProfileController.CreateProfile(session, body!);
            return resultPacket.Send();

        });

        // GET /profiles/:id
        app.MapGet("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            await ValidatorSessionMiddleware.TryValidateSession(request);
            var resultPacket = await ProfileController.GetProfile(queryParamId, false);
            return resultPacket.Send();

        });

        // PUT /profiles/:id
        app.MapPut("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            await ValidatorSessionMiddleware.ValidateSessionAdmin(request);
            var body = await ValidatorBodyMiddleware.ValidateBody(request, ProfileBodyValidatorsTemplate.Update());
            var resultPacket = await ProfileController.UpdateProfile(queryParamId, body!);
            return resultPacket.Send();

        });

        // DELETE /profiles/:id
        app.MapDelete("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            await ValidatorSessionMiddleware.ValidateSessionAdmin(request);
            var resultPacket = await ProfileController.DeleteProfile(queryParamId);
            return resultPacket.Send();

        });

        return group;
    }
}