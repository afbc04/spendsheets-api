using System.Text.Json;

public static class ValidatorBodyMiddleware
{
    public static async Task<Dictionary<string, object?>?> ValidateBody(HttpRequest request, RequestBodyTemplate rbt)
    {
        string RequestBody;

        using (var reader = new StreamReader(request.Body))
        {
            RequestBody = await reader.ReadToEndAsync();
        }

        bool IsEmpty = string.IsNullOrWhiteSpace(RequestBody);

            if (rbt.IsRequired && IsEmpty)
                throw new ValidatorBodyMiddlewareException(ValidatorBodyMiddlewareExceptionEnum.RequiredBody);

            if (!IsEmpty)
                return _GetData(RequestBody, rbt.body);

        return null;
    }

    private static Dictionary<string, object?> _GetData(string body, Dictionary<string, ValidatorBodyFieldMiddleware> validator) {

            try {

                using var doc = JsonDocument.Parse(body);
                var json = doc.RootElement;

                if (json.ValueKind != JsonValueKind.Object)
                   throw new ValidatorBodyMiddlewareException(ValidatorBodyMiddlewareExceptionEnum.NotJSON);

                List<string> requiredFieldsMissing = [];
                Dictionary<string,string> wrongDatatypes = [];
                var data = _ValidateFieldsRecursive(json, validator, ref requiredFieldsMissing, ref wrongDatatypes, "");
                
                if (requiredFieldsMissing.Count > 0)
                    throw new ValidatorBodyMiddlewareException(ValidatorBodyMiddlewareExceptionEnum.RequiredFieldMissing, requiredFieldsMissing);

                if (wrongDatatypes.Keys.Count > 0)
                    throw new ValidatorBodyMiddlewareException(ValidatorBodyMiddlewareExceptionEnum.WrongDatatypeField, wrongDatatypes);

                return data;
            }
            catch (ValidatorBodyMiddlewareException) 
            {
                throw;
            }
            catch (Exception)
            {
                throw new ValidatorBodyMiddlewareException(ValidatorBodyMiddlewareExceptionEnum.InvalidJSON);
            }
        }

        private static Dictionary<string, object?> _ValidateFieldsRecursive(JsonElement element, Dictionary<string, ValidatorBodyFieldMiddleware> validator, ref List<string> requiredFieldsMissing, ref Dictionary<string,string> wrongDatatypes, string path) {

            Dictionary<string, object?> Data = [];

            foreach (KeyValuePair<string, ValidatorBodyFieldMiddleware> de in validator)
            {
                string NewPath = $"{path}{de.Key}";
                
                if (!element.TryGetProperty(de.Key, out JsonElement property))
                {
                    if (de.Value.IsRequired && !de.Value.AllowNull)
                    {
                        requiredFieldsMissing.Add(NewPath);
                        continue;
                    }
                }
                else
                {
                    if (de.Value.IsList && property.ValueKind != JsonValueKind.Array)
                    {
                        wrongDatatypes[NewPath] = "list";
                        continue;
                    }

                    List<JsonElement> ValuesToAnalyse = property.ValueKind == JsonValueKind.Array
                        ? [.. property.EnumerateArray()]
                        : [property];

                    int ListLength = ValuesToAnalyse.Count;
                    for (int i = 0; i < ListLength; i++) {

                        var item = ValuesToAnalyse[i];

                        if (de.Value.IsList)
                            path += $"[{i}]";

                        Data[de.Key] = de.Value is ValidatorBodyObjectMiddleware middleware
                            ? _ValidateFieldsRecursive(item, middleware.obj, ref requiredFieldsMissing, ref wrongDatatypes, NewPath)
                            : _ObtainItemValue(item, ((ValidatorBodyItemMiddleware) de.Value).DateType, ref wrongDatatypes, de.Value.AllowNull, NewPath);
                    }
                }
            }

            return Data;
        }

        private static object? _ObtainItemValue(JsonElement element, ValidatorBodyFieldTypeMiddleware type, ref Dictionary<string,string> wrongDatatypes, bool canBeNull, string path) {

            if (element.ValueKind == JsonValueKind.Null) {
                
                if (!canBeNull)
                    wrongDatatypes[path] = ValidatorBodyFieldMiddleware.GetTypeName(type,canBeNull);

                return null;
            }

            try
            {
                return type switch
                {
                    ValidatorBodyFieldTypeMiddleware.Integer => element.GetInt64(),
                    ValidatorBodyFieldTypeMiddleware.Float => element.GetDouble(),
                    ValidatorBodyFieldTypeMiddleware.String => element.GetString(),
                    ValidatorBodyFieldTypeMiddleware.Date => DateOnly.FromDateTime(element.GetDateTime()),
                    ValidatorBodyFieldTypeMiddleware.DateTime => element.GetDateTime(),
                    ValidatorBodyFieldTypeMiddleware.Boolean => element.GetBoolean(),
                    _ => null
                };
            }
            catch
            {
                wrongDatatypes[path] = ValidatorBodyFieldMiddleware.GetTypeName(type,canBeNull);
                return null;
            }
        }

} 
