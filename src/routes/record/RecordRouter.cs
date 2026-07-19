public static class RecordRouters {

    public static RouteGroupBuilder RecordRoutersMapping(this RouteGroupBuilder group) {

        var app = group.MapGroup("/records").AllowAnonymous();

        // GET /v1.0/categories
        app.MapGet("", async (HttpRequest request) => {

            var session = await ValidatorSessionMiddleware.TryValidateSession(request);
            QueryPage page = await ValidatorPageMiddleware.ValidatePage(request, ["id","date","value","createdAt","updatedAt"]);
            var filtersQuery = await ValidatorQueryStringMiddleware.ValidateQueryString(request, RecordQueryStringValidatorsTemplate.List());
            var resultPacket = await RecordController.ListRecords(page, filtersQuery, session is null);
            return resultPacket.Send();

        });

        // POST /v1.0/categories
        app.MapPost("", async (HttpRequest request) => {

            await ValidatorSessionMiddleware.ValidateSession(request);
            var body = await ValidatorBodyMiddleware.ValidateBody(request, RecordBodyValidatorsTemplate.Create());
            var resultPacket = await RecordController.CreateRecord(body!);
            return resultPacket.Send();

        });

        // GET /v1.0/categories/:id
        app.MapGet("{queryParamId}", async (HttpRequest request, string queryParamId) => { 

            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            var session = await ValidatorSessionMiddleware.TryValidateSession(request);
            var resultPacket = await RecordController.GetRecord(id, session is null);
            return resultPacket.Send();

        });

        // DELETE /v1.0/categories/:id
        app.MapDelete("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            await ValidatorSessionMiddleware.ValidateSession(request);
            var resultPacket = await RecordController.DeleteRecord(id);
            return resultPacket.Send();

        });

        // PUT /v1.0/categories/:id
        app.MapPut("{queryParamId}", async (HttpRequest request, string queryParamId) => {

            long id = await ValidatorQueryParamMiddleware.ValidateNumericalID(queryParamId);
            await ValidatorSessionMiddleware.ValidateSession(request);
            var body = await ValidatorBodyMiddleware.ValidateBody(request, RecordBodyValidatorsTemplate.Update());
            var resultPacket = await RecordController.UpdateRecord(id,body!);
            return resultPacket.Send();

        });

        return group;
    }
}