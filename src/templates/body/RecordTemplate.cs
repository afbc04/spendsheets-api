public static class RecordBodyValidatorsTemplate
{
    public static RequestBodyTemplate Create() => create;
    public static RequestBodyTemplate Update() => update;

    private static readonly RequestBodyTemplate create = RequestBodyTemplate.Required(new()
    {
        ["note"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
        ["value"] = ValidatorBodyItemMiddleware.RequiredNotNull(ValidatorBodyFieldTypeMiddleware.Float),
        ["date"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Date),
        ["workspace"] = ValidatorBodyItemMiddleware.RequiredNotNull(ValidatorBodyFieldTypeMiddleware.Integer),
        ["public"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Boolean),
        ["invisible"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Boolean),
        ["status"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.String)
    });

    private static readonly RequestBodyTemplate update = RequestBodyTemplate.Required(new()
    {
        ["note"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
        ["value"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Float),
        ["date"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Date),
        ["workspace"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Integer),
        ["public"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Boolean),
        ["invisible"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Boolean),
        ["status"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.String)
    });

}
