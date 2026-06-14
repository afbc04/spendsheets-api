public static class SessionBodyValidatorsTemplate
{
    public static RequestBodyTemplate Obtain() => obtain;

    private static readonly RequestBodyTemplate obtain = RequestBodyTemplate.Required(new()
    {
        ["username"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["password"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
    });
}
