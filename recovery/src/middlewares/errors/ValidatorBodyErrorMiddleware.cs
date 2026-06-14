public static class ValidatorBodyErrorMiddleware
{
    public static async Task<SendingPacket> Handle(HttpContext context, ValidatorBodyMiddlewareException ex)
    {
        context.Response.ContentType = "application/json";

        switch (ex.Type)
        {
            case ValidatorBodyMiddlewareExceptionEnum.RequiredBody:
                return SendingPacket.Error(ErrorCategory.REQUEST_BODY_REQUIRED);

            case ValidatorBodyMiddlewareExceptionEnum.NotJSON:
                return SendingPacket.Error(ErrorCategory.REQUEST_BODY_NOT_JSON);

            case ValidatorBodyMiddlewareExceptionEnum.InvalidJSON:
                return SendingPacket.Error(ErrorCategory.REQUEST_BODY_INVALID_JSON);

            case ValidatorBodyMiddlewareExceptionEnum.RequiredFieldMissing:
                return SendingPacket.Error(ErrorCategory.REQUEST_BODY_REQUIRED_FIELDS_MISSING,
                    new Dictionary<string, object?>
                    {
                        ["requiredFieldsMissing"] = ex.RequiredFieldsMissing
                    });
                
            case ValidatorBodyMiddlewareExceptionEnum.WrongDatatypeField:
                return SendingPacket.Error(ErrorCategory.REQUEST_BODY_WRONG_DATATYPES,
                    new Dictionary<string, object?>
                    {
                        ["wrongDatatypes"] = ex.WrongDatatypeFields
                    });

            default:
                return SendingPacket.Error(ErrorCategory.REQUEST_BODY_ERROR);
        }
    }
}