public static class WorkspaceBodyValidatorsTemplate
{
    public static RequestBodyTemplate Create() => create;
    public static RequestBodyTemplate Update() => update;

    private static readonly RequestBodyTemplate create = RequestBodyTemplate.Required(new()
    {
        ["name"] = ValidatorBodyItemMiddleware.RequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["description"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
        ["initialMoney"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Float),
    });

    private static readonly RequestBodyTemplate update = RequestBodyTemplate.Required(new()
    {
        ["name"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["description"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
        ["initialMoney"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Float),
    });

}
