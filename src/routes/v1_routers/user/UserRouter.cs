public static class UserRouters {

    public static RouteGroupBuilder UserRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/user").AllowAnonymous();
/*
        // GET /v1.0/user
        app.MapGet("", async (HttpRequest request) => {

            var resultPacket = await UserController.GetUser(false);
            return resultPacket.Send();

        });

        // POST /v1.0/user
        app.MapPost("", async (HttpRequest request) => {

            var body = await ValidatorBodyMiddleware.ValidateBody(request, UserBodyValidatorsTemplate.Create());
            var resultPacket = await UserController.CreateUser(body!);
            return resultPacket.Send();

        });

        // PUT /v1.0/user
        app.MapPut("", async (HttpRequest request) => {

            var body = await ValidatorBodyMiddleware.ValidateBody(request, UserBodyValidatorsTemplate.Update());
            var resultPacket = await UserController.UpdateUser(body!);
            return resultPacket.Send();

        });

        // PATCH /v1.0/user
        app.MapPatch("", async (HttpRequest request) => {

            var body = await ValidatorBodyMiddleware.ValidateBody(request, UserBodyValidatorsTemplate.Patch());
            var resultPacket = await UserController.PatchUser(body!);
            return resultPacket.Send();

        });*/

        return group;
    }
}