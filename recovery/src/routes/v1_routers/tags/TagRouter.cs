public static class TagRouters {

    public static RouteGroupBuilder TagRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/tags").AllowAnonymous();
/*
        // GET /v1.0/tags
        app.MapGet("", async (HttpRequest request) => {

            await ValidatorConfigMiddleware.VerifyIfUserExists();
            string? token = await ValidatorTokenMiddleware.TryExtractToken(request);
            QueryPage page = await ValidatorPageMiddleware.ValidatePage(request, ["id","name"]);
            var filtersQuery = await ValidatorQueryStringMiddleware.ValidateQueryString(request,TagQueryStringValidatorsTemplate.List());
            var resultPacket = await TagsController.ListTags(page, filtersQuery, token is null);
            return resultPacket.Send();

        });

        // POST /v1.0/tags
        app.MapPost("", async (HttpRequest request) => {

            await ValidatorConfigMiddleware.VerifyIfUserExists();
            await ValidatorTokenMiddleware.ValidateToken(request);
            var body = await ValidatorBodyMiddleware.ValidateBody(request, TagBodyValidatorsTemplate.Create());
            var resultPacket = await TagsController.CreateTag(body!);
            return resultPacket.Send();

        });

        // GET /v1.0/tags/:id
        app.MapGet("{queryParamId}", async (HttpRequest request, string queryParamId) => { 

            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            await ValidatorConfigMiddleware.VerifyIfUserExists();
            string? token = await ValidatorTokenMiddleware.TryExtractToken(request);
            var resultPacket = await TagsController.GetTag(id, token is null);
            return resultPacket.Send();

        });

        // DELETE /v1.0/tags/:id
        app.MapDelete("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            await ValidatorConfigMiddleware.VerifyIfUserExists();
            await ValidatorTokenMiddleware.ValidateToken(request);
            var resultPacket = await TagsController.DeleteTag(id);
            return resultPacket.Send();

        });

        
        // PATCH /v1.0/tags/:id
        app.MapPatch("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            await ValidatorConfigMiddleware.VerifyIfUserExists();
            await ValidatorTokenMiddleware.ValidateToken(request);
            var body = await ValidatorBodyMiddleware.ValidateBody(request, TagBodyValidatorsTemplate.Patch());
            var resultPacket = await TagsController.PatchTag(id,body!);
            return resultPacket.Send();

        });

        // PUT /v1.0/tags/:id
        app.MapPut("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            await ValidatorConfigMiddleware.VerifyIfUserExists();
            await ValidatorTokenMiddleware.ValidateToken(request);
            var body = await ValidatorBodyMiddleware.ValidateBody(request, TagBodyValidatorsTemplate.Update());
            var resultPacket = await TagsController.UpdateTag(id,body!);
            return resultPacket.Send();

        });*/

        return group;
    }
}