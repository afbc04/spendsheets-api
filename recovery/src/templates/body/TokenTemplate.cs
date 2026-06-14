public static class TokenBodyValidatorsTemplate
{
    public static RequestBodyTemplate Obtain() => obtain;

    private static readonly RequestBodyTemplate obtain = RequestBodyTemplate.Required(new()
    {
        ["username"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["password"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["refreshToken"] = ValidatorBodyItemMiddleware.NotRequiredNotNull(ValidatorBodyFieldTypeMiddleware.String),
        ["grantType"] = ValidatorBodyItemMiddleware.RequiredNotNull(ValidatorBodyFieldTypeMiddleware.String)
    });
}
