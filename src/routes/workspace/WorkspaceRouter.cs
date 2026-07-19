public static class WorkspacesRouters {

    public static RouteGroupBuilder WorkspacesRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/workspaces").AllowAnonymous();

        // GET /workspaces
        app.MapGet("", async (HttpRequest request) => {

            await ValidatorSessionMiddleware.TryValidateSession(request);
            var resultPacket = await WorkspaceController.ListWorkspace(false);
            return resultPacket.Send();

        });

        // POST /workspaces
        app.MapPost("", async (HttpRequest request) => {

            await ValidatorSessionMiddleware.ValidateSessionAdmin(request);
            var body = await ValidatorBodyMiddleware.ValidateBody(request, WorkspaceBodyValidatorsTemplate.Create());
            var resultPacket = await WorkspaceController.CreateWorkspace(body!);
            return resultPacket.Send();

        });

        // GET /workspaces/:id
        app.MapGet("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            await ValidatorSessionMiddleware.TryValidateSession(request);
            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            var resultPacket = await WorkspaceController.GetWorkspace(id, false);
            return resultPacket.Send();

        });

        // PUT /workspaces/:id
        app.MapPut("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            await ValidatorSessionMiddleware.ValidateSessionAdmin(request);
            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            var body = await ValidatorBodyMiddleware.ValidateBody(request, WorkspaceBodyValidatorsTemplate.Update());
            var resultPacket = await WorkspaceController.UpdateWorkspace(id, body!);
            return resultPacket.Send();

        });

        // DELETE /workspaces/:id
        app.MapDelete("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            await ValidatorSessionMiddleware.ValidateSessionAdmin(request);
            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            var resultPacket = await WorkspaceController.DeleteWorkspace(id);
            return resultPacket.Send();

        });

        return group;
    }
}