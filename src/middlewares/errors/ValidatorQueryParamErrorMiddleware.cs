public static class ValidatorQueryParamErrorMiddleware
{
    public static async Task Handle(HttpContext context, ValidatorQueryParamMiddlewareException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 417;
        await context.Response.WriteAsJsonAsync(new
        {
            error = ex.Message
        });
    }
}