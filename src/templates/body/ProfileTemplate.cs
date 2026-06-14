public static class ProfileBodyValidatorsTemplate
{
    public static RequestBodyTemplate Create() => create;
    public static RequestBodyTemplate Update() => update;

    private static readonly RequestBodyTemplate create = RequestBodyTemplate.Required(new()
    {
        ["username"] = ValidatorBodyItemMiddleware.RequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["name"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
        ["password"] = ValidatorBodyItemMiddleware.RequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["active"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Boolean),
    });

    private static readonly RequestBodyTemplate update = RequestBodyTemplate.Required(new()
    {
        ["name"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
        ["password"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["active"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Boolean),
    });

}
