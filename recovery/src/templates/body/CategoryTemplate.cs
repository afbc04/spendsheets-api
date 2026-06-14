public static class CategoryBodyValidatorsTemplate
{
    public static RequestBodyTemplate Create() => create;
    public static RequestBodyTemplate Update() => update;
    public static RequestBodyTemplate Patch() => patch;

    private static readonly RequestBodyTemplate create = RequestBodyTemplate.Required(new()
    {
        ["name"] = ValidatorBodyItemMiddleware.RequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["description"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.String),
        ["parentId"] = ValidatorBodyItemMiddleware.NotRequiredNull(ValidatorBodyFieldTypeMiddleware.Integer)
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
