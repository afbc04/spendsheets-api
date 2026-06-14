using System.Text.RegularExpressions;

public static partial class ValidatorPageMiddleware
{
    public static async Task<QueryPage> ValidatePage(HttpRequest request, List<string> validSortFields)
    {
        try
        {
            List<string> pageSortInvalidFields = [];
            var page = _GetPage(request.Query, validSortFields, ref pageSortInvalidFields);

            if (pageSortInvalidFields.Count > 0)
                throw new ValidatorQueryStringMiddlewareException(ValidatorQueryStringMiddlewareExceptionEnum.NotValidSortField, pageSortInvalidFields);

            return page;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static QueryPage _GetPage(IQueryCollection query, List<string> validSortFields, ref List<string> pageSortInvalidFields)
    {
        long page = 1;
        long limit = PageRules.LimitDefault;
        List<QueryPageOrderItem> sort = [];

        if (query.TryGetValue("page", out var pageValue))
        {
            if (long.TryParse(pageValue, out long pageValueCast))
            {
                if (pageValueCast <= 0)
                    throw new ValidatorQueryStringMiddlewareException(ValidatorQueryStringMiddlewareExceptionEnum.PageMin);

                page = pageValueCast;
            }
            else
                throw new ValidatorQueryStringMiddlewareException(ValidatorQueryStringMiddlewareExceptionEnum.InvalidPage);
        }

        if (query.TryGetValue("limit", out var limitValue))
        {
            if (long.TryParse(limitValue, out long limitValueCast))
            {
                if (limitValueCast < PageRules.LimitMin)
                    throw new ValidatorQueryStringMiddlewareException(ValidatorQueryStringMiddlewareExceptionEnum.LimitMin);

                if (limitValueCast > PageRules.LimitMax)
                    throw new ValidatorQueryStringMiddlewareException(ValidatorQueryStringMiddlewareExceptionEnum.LimitMax);

                limit = limitValueCast;
            }
            else
                throw new ValidatorQueryStringMiddlewareException(ValidatorQueryStringMiddlewareExceptionEnum.InvalidLimit);
        }

        if (validSortFields.Count > 0 && query.TryGetValue("sort", out var sortValue))
        {
            string sortValueCast = sortValue.ToString();

            if (SortRegex().IsMatch(sortValueCast) == false)
                throw new ValidatorQueryStringMiddlewareException(ValidatorQueryStringMiddlewareExceptionEnum.InvalidSortQuery);

            foreach (string token in sortValueCast.Split(","))
            {
                string[] tokenArgs = token.Split(":");
                bool isAsc = Convert.ToInt32(tokenArgs[1]) == 1;

                if (!validSortFields.Contains(tokenArgs[0].ToLower()))
                    pageSortInvalidFields.Add(tokenArgs[0]);
                else
                    sort.Add(new QueryPageOrderItem(tokenArgs[0], isAsc));
            }
        }

        return new QueryPage(page,limit,sort);
    }

    [GeneratedRegex(@"^(\w[\w_]*:-?1)(,(\w[\w_]*:-?1))*$")]
    private static partial Regex SortRegex();
}
