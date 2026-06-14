public static class ValidatorBodyErrorMiddleware
{
    public static async Task<SendingPacket> Handle(HttpContext context, ValidatorBodyMiddlewareException ex)
    {
        context.Response.ContentType = "application/json";

        switch (ex.Type)
        {
            case ValidatorBodyMiddlewareExceptionEnum.RequiredBody:
                return SendingPacket.Error(400, "Request's body is required.");

            case ValidatorBodyMiddlewareExceptionEnum.NotJSON:
                return SendingPacket.Error(415, "Request's body  is not a JSON.");

            case ValidatorBodyMiddlewareExceptionEnum.InvalidJSON:
                return SendingPacket.Error(400, "Request's body is not a valid JSON.");

            case ValidatorBodyMiddlewareExceptionEnum.RequiredFieldMissing:
                return SendingPacket.Error(400, "Required fields are missing",
                    new Dictionary<string, object?>
                    {
                        ["requiredFieldsMissing"] = ex.RequiredFieldsMissing
                    });
                
            case ValidatorBodyMiddlewareExceptionEnum.WrongDatatypeField:
                return SendingPacket.Error(400, "Some fields have the wrong datatype",
                    new Dictionary<string, object?>
                    {
                        ["wrongDatatypes"] = ex.WrongDatatypeFields
                    });

            default:
                return SendingPacket.Error(500, "Request's body validator couldn't handle this request.");
        }
    }
}