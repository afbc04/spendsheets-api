using Queries;

public class QTOHelper {

    public static Query getQuery(QueriesRequest? request, Func<QueriesRequest,Query> action) {
        return request == null ? new Query(PageRules.limit_default,PageRules.page_default) : action(request!);
    }

}