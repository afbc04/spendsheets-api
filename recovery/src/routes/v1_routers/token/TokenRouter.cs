public static class TokenRouters {

    public static RouteGroupBuilder TokenRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/token").AllowAnonymous();
/*
        // GET /v1.0/token
        app.MapGet("", async (HttpRequest request) => {

            string token = await ValidatorTokenMiddleware.ValidateToken(request);
            var resultPacket = await TokenController.ValidateToken(token);
            return resultPacket.Send();

        });

        // POST /v1.0/token
        app.MapPost("", async (HttpRequest request) => {

            await ValidatorConfigMiddleware.VerifyIfUserExists();
            var body = await ValidatorBodyMiddleware.ValidateBody(request, TokenBodyValidatorsTemplate.Obtain());
            var resultPacket = await TokenController.RenewToken(body!);
            return resultPacket.Send();

        });

        // DELETE /v1.0/token
        app.MapDelete("", async (HttpRequest request) => {

            string token = await ValidatorTokenMiddleware.ValidateToken(request);
            var resultPacket = await TokenController.DeleteToken(token);
            return resultPacket.Send();

        });*/

        return group;
    }

}