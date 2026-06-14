public static class RecordRouters {

    public static RouteGroupBuilder RecordRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/records").AllowAnonymous();

        /*
        // GET /v1.0/categories
        app.MapGet("", async (HttpRequest request) => {

            await ValidatorConfigMiddleware.VerifyIfUserExists();
            string? token = await ValidatorTokenMiddleware.TryExtractToken(request);
            QueryPage page = await ValidatorPageMiddleware.ValidatePage(request, ["id","name"]);
            var filtersQuery = await ValidatorQueryStringMiddleware.ValidateQueryString(request,CategoryQueryStringValidatorsTemplate.List());
            var resultPacket = await CategoryController.ListCategories(page, filtersQuery, token is null);
            return resultPacket.Send();

        });*/
/*
        // POST /v1.0/records
        app.MapPost("", async (HttpRequest request) => {

            await ValidatorConfigMiddleware.VerifyIfUserExists();
            await ValidatorTokenMiddleware.ValidateToken(request);
            var body = await ValidatorBodyMiddleware.ValidateBody(request, RecordBodyValidatorsTemplate.Create());
            var resultPacket = await RecordController.CreateRecord(body!);
            return resultPacket.Send();

        });
/*
        // GET /v1.0/categories/:id
        app.MapGet("{queryParamId}", async (HttpRequest request, string queryParamId) => { 

            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            await ValidatorConfigMiddleware.VerifyIfUserExists();
            string? token = await ValidatorTokenMiddleware.TryExtractToken(request);
            var resultPacket = await CategoryController.GetCategory(id, token is null);
            return resultPacket.Send();

        });

        // DELETE /v1.0/categories/:id
        app.MapDelete("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            await ValidatorConfigMiddleware.VerifyIfUserExists();
            await ValidatorTokenMiddleware.ValidateToken(request);
            var resultPacket = await CategoryController.DeleteCategory(id);
            return resultPacket.Send();

        });

        
        // PATCH /v1.0/categories/:id
        app.MapPatch("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            await ValidatorConfigMiddleware.VerifyIfUserExists();
            await ValidatorTokenMiddleware.ValidateToken(request);
            var body = await ValidatorBodyMiddleware.ValidateBody(request, CategoryBodyValidatorsTemplate.Patch());
            var resultPacket = await CategoryController.PatchCategory(id,body!);
            return resultPacket.Send();

        });

        // PUT /v1.0/categories/:id
        app.MapPut("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            await ValidatorConfigMiddleware.VerifyIfUserExists();
            await ValidatorTokenMiddleware.ValidateToken(request);
            var body = await ValidatorBodyMiddleware.ValidateBody(request, CategoryBodyValidatorsTemplate.Update());
            var resultPacket = await CategoryController.UpdateCategory(id,body!);
            return resultPacket.Send();

        });*/

        return group;
    }

}