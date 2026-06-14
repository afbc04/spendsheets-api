public static class RecordBodyValidatorsTemplate
{
    public static RequestBodyTemplate Create() => create;
    //public static RequestBodyTemplate Update() => update;
    //public static RequestBodyTemplate Patch() => patch;

    private static readonly RequestBodyTemplate create = RequestBodyTemplate.Required(new()
    {
        ["description"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
        ["date"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Date),
        ["invisible"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Boolean),
        ["public"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Boolean),
        ["status"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["products"] = ValidatorBodyObjectMiddleware.RequiredNotNullList(new()
        {
            ["money"] = ValidatorBodyItemMiddleware.RequiredNotNull(ValidatorBodyFieldTypeMiddleware.Float),
            ["note"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
            ["category"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Integer),
            ["tags"] = ValidatorBodyItemMiddleware.NotRequiredNotNullList(ValidatorBodyFieldTypeMiddleware.Integer),
            ["goals"] = ValidatorBodyItemMiddleware.NotRequiredNotNullList(ValidatorBodyFieldTypeMiddleware.Integer)
        })
    });

    private static readonly RequestBodyTemplate update = RequestBodyTemplate.Required(new()
    {
        ["name"] = ValidatorBodyItemMiddleware.RequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["description"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
        ["parentId"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.Integer)
    });

    private static readonly RequestBodyTemplate patch = RequestBodyTemplate.Required(new()
    {
        ["name"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["description"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
        ["parentId"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.Integer)
    });

}
