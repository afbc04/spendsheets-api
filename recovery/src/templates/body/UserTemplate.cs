public static class UserBodyValidatorsTemplate
{
    public static RequestBodyTemplate Create() => create;
    public static RequestBodyTemplate Update() => update;
    public static RequestBodyTemplate Patch() => patch;

    private static readonly RequestBodyTemplate create = RequestBodyTemplate.Required(new()
    {
        ["username"] = ValidatorBodyItemMiddleware.RequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["name"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
        ["initialMoney"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Integer),
        ["password"] = ValidatorBodyItemMiddleware.RequiredNotNull(ValidatorBodyFieldTypeMiddleware.String)
    });

    private static readonly RequestBodyTemplate update = RequestBodyTemplate.Required(new()
    {
        ["username"] = ValidatorBodyItemMiddleware.RequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["name"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
        ["initialMoney"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Integer),
        ["password"] = ValidatorBodyItemMiddleware.RequiredNotNull(ValidatorBodyFieldTypeMiddleware.String)
    });

    private static readonly RequestBodyTemplate patch = RequestBodyTemplate.Required(new()
    {
        ["username"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["name"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
        ["initialMoney"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.Integer),
        ["password"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.String)
    });

}
