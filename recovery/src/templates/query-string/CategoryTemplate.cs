public static class CategoryQueryStringValidatorsTemplate
{
    public static RequestQueryStringTemplate List() => list;

    private static readonly RequestQueryStringTemplate list = new(
        new()
        {
            ["name"] = ValidatorQueryStringItemMiddleware.NotNull(ValidatorQueryStringFieldTypeMiddleware.String),
            ["parentId"] = ValidatorQueryStringItemMiddleware.NotNull(ValidatorQueryStringFieldTypeMiddleware.Integer),
            ["subcategory"] = ValidatorQueryStringItemMiddleware.NotNull(ValidatorQueryStringFieldTypeMiddleware.Boolean)
        }
    );
}
