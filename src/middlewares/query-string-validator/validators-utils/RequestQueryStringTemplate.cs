public class RequestQueryStringTemplate
{
    public Dictionary<string, ValidatorQueryStringFieldMiddleware> queries { get; set; }

    public RequestQueryStringTemplate(Dictionary<string, ValidatorQueryStringFieldMiddleware> queries)
    {
        this.queries = queries;
    }
}
