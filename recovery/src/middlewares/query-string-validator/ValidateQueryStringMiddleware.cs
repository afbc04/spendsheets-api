using System.Text.Json;

public static class ValidatorQueryStringMiddleware
{
    public static async Task<Dictionary<string, object?>> ValidateQueryString(HttpRequest request, RequestQueryStringTemplate rqst)
    {
        try
        {
            Dictionary<string, string> wrongDatatypes = [];
            var queries = _GetQueryStrings(request.Query, rqst.queries, ref wrongDatatypes, "");

            if (wrongDatatypes.Keys.Count > 0)
                throw new ValidatorQueryStringMiddlewareException(ValidatorQueryStringMiddlewareExceptionEnum.WrongDatatypeField, wrongDatatypes);

            return queries;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static Dictionary<string, object?> _GetQueryStrings(IQueryCollection query, Dictionary<string, ValidatorQueryStringFieldMiddleware> validator, ref Dictionary<string, string> wrongDatatypes, string path)
    {
        Dictionary<string, object?> Data = [];

        foreach (KeyValuePair<string, ValidatorQueryStringFieldMiddleware> de in validator)
        {
            string NewPath = $"{path}{de.Key}";

            if (query.TryGetValue(de.Key, out var value))
            {
                string[] ValuesToAnalyse = de.Value.IsList
                    ? value.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    : [value.ToString()];

                int ListLength = ValuesToAnalyse.Length;
                for (int i = 0; i < ListLength; i++)
                {
                    var item = ValuesToAnalyse[i];

                    if (de.Value.IsList)
                        path += $"[{i}]";

                    Data[de.Key] = _ObtainItemValue(item, ((ValidatorQueryStringItemMiddleware)de.Value).DateType, ref wrongDatatypes, de.Value.AllowNull, NewPath);
                }
            }
        }

        return Data;
    }

    private static object? _ObtainItemValue(string value, ValidatorQueryStringFieldTypeMiddleware type, ref Dictionary<string, string> wrongDatatypes, bool canBeNull, string path)
    {
        if (canBeNull && (string.IsNullOrEmpty(value) || value.Equals("null", StringComparison.CurrentCultureIgnoreCase)))
            return null;

        try
        {
            return type switch
            {
                ValidatorQueryStringFieldTypeMiddleware.Integer => long.Parse(value),
                ValidatorQueryStringFieldTypeMiddleware.Float => double.Parse(value),
                ValidatorQueryStringFieldTypeMiddleware.String => value,
                ValidatorQueryStringFieldTypeMiddleware.Date => DateOnly.Parse(value),
                ValidatorQueryStringFieldTypeMiddleware.DateTime => DateTime.Parse(value),
                ValidatorQueryStringFieldTypeMiddleware.Boolean => bool.Parse(value),
                _ => null
            };
        }
        catch
        {
            wrongDatatypes[path] = ValidatorQueryStringFieldMiddleware.GetTypeName(type, canBeNull);
            return null;
        }
    }
}
