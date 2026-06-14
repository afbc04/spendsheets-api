public class ErrorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorMiddleware> _logger;

    public ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidatorBodyMiddlewareException ex)
        {
            var packet = await ValidatorBodyErrorMiddleware.Handle(context,ex);
            await packet.Send().ExecuteAsync(context);
        }
        catch (ValidatorQueryStringMiddlewareException ex)
        {
            var packet = await ValidatorQueryStringErrorMiddleware.Handle(context,ex);
            await packet.Send().ExecuteAsync(context);
        }
        catch (ValidatorTokenConfigMiddlewareException ex)
        {
            var packet = await ValidatorTokenConfigErrorMiddleware.Handle(context,ex);
            await packet.Send().ExecuteAsync(context);
        }
        catch (ValidatorQueryParamMiddlewareException ex)
        {
            await ValidatorQueryParamErrorMiddleware.Handle(context,ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var response = new
            {
                error = "Internal Server Error",
                message = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}