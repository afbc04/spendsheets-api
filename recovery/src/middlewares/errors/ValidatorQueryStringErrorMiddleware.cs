public static class ValidatorQueryStringErrorMiddleware
{
    public static async Task<SendingPacket> Handle(HttpContext context, ValidatorQueryStringMiddlewareException ex)
    {
        context.Response.ContentType = "application/json";

        switch (ex.Type)
        {
            case ValidatorQueryStringMiddlewareExceptionEnum.WrongDatatypeField:
                return SendingPacket.Error(ErrorCategory.REQUEST_QUERY_WRONG_DATATYPES,
                    new Dictionary<string, object?>
                    {
                        ["wrongDatatypes"] = ex.WrongDatatypeFields
                    });

            case ValidatorQueryStringMiddlewareExceptionEnum.NotValidSortField:
                return SendingPacket.Error(ErrorCategory.REQUEST_PAGE_INVALID_SORT_FIELDS,
                    new Dictionary<string, object?>
                    {
                        ["invalidSortFields"] = ex.PageSortInvalidFields
                    });

            case ValidatorQueryStringMiddlewareExceptionEnum.PageMin:
                return SendingPacket.Error(ErrorCategory.REQUEST_PAGE_PAGE_MIN);

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

            default:
                return SendingPacket.Error(ErrorCategory.REQUEST_QUERY_ERROR);
        }
    }
}