public static class RecordQueryStringValidatorsTemplate
{
    public static RequestQueryStringTemplate List() => list;

    private static readonly RequestQueryStringTemplate list = new(
        new()
        {
            ["status"] = ValidatorQueryStringItemMiddleware.NotNullList(ValidatorQueryStringFieldTypeMiddleware.String),
            ["workspace"] = ValidatorQueryStringItemMiddleware.NotNull(ValidatorQueryStringFieldTypeMiddleware.Integer),
            ["minDate"] = ValidatorQueryStringItemMiddleware.NotNull(ValidatorQueryStringFieldTypeMiddleware.Date),
            ["maxDate"] = ValidatorQueryStringItemMiddleware.NotNull(ValidatorQueryStringFieldTypeMiddleware.Date),
            ["public"] = ValidatorQueryStringItemMiddleware.NotNull(ValidatorQueryStringFieldTypeMiddleware.Boolean),
            ["invisible"] = ValidatorQueryStringItemMiddleware.NotNull(ValidatorQueryStringFieldTypeMiddleware.Boolean),
            ["onlyRevenues"] = ValidatorQueryStringItemMiddleware.NotNull(ValidatorQueryStringFieldTypeMiddleware.Boolean),
            ["onlyExpenses"] = ValidatorQueryStringItemMiddleware.NotNull(ValidatorQueryStringFieldTypeMiddleware.Boolean)
        }
    );
}