public static class TagQueryStringValidatorsTemplate
{
    public static RequestQueryStringTemplate List() => list;

    private static readonly RequestQueryStringTemplate list = new RequestQueryStringTemplate(
        new()
        {
            ["name"] = ValidatorQueryStringItemMiddleware.NotNull(ValidatorQueryStringFieldTypeMiddleware.String),
        }
    );
}
