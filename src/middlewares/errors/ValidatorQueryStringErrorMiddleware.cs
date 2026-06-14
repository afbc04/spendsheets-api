public static class ValidatorQueryStringErrorMiddleware
{
    public static async Task<SendingPacket> Handle(HttpContext context, ValidatorQueryStringMiddlewareException ex)
    {
        context.Response.ContentType = "application/json";

        switch (ex.Type)
        {
            /*
            case ValidatorQueryStringMiddlewareExceptionEnum.WrongDatatypeField:
                return SendingPacket.Error(400, "Some fields in query string have the wrong datatype",
                    new Dictionary<string, object?>
                    {
                        ["wrongDatatypes"] = ex.WrongDatatypeFields
                    });

            case ValidatorQueryStringMiddlewareExceptionEnum.NotValidSortField:
                return SendingPacket.Error(400, ""
                    new Dictionary<string, object?>
                    {
                        ["invalidSortFields"] = ex.PageSortInvalidFields
                    });

            case ValidatorQueryStringMiddlewareExceptionEnum.PageMin:
                return SendingPacket.Error(400, $"Minimum page is {}");

            case ValidatorQueryStringMiddlewareExceptionEnum.LimitMin:
                return SendingPacket.Error(ErrorCategory.REQUEST_PAGE_LIMIT_MIN);

            case ValidatorQueryStringMiddlewareExceptionEnum.LimitMax:
                return SendingPacket.Error(ErrorCategory.REQUEST_PAGE_LIMIT_MAX);

            case ValidatorQueryStringMiddlewareExceptionEnum.InvalidPage:
                return SendingPacket.Error(ErrorCategory.REQUEST_PAGE_INVALID_PAGE);

            case ValidatorQueryStringMiddlewareExceptionEnum.InvalidLimit:
                return SendingPacket.Error(ErrorCategory.REQUEST_PAGE_INVALID_LIMIT);

            case ValidatorQueryStringMiddlewareExceptionEnum.InvalidSortQuery:
                return SendingPacket.Error(ErrorCategory.REQUEST_PAGE_INVALID_SORT_QUERY);
*/
            default:
                return SendingPacket.Error(500, "Query string validator couldn't handle this request.");
        }
    }
}