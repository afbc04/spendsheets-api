using Queries;

public class QTO {

    public static Query Tag(QueriesRequest? request) {
        return QTOHelper.getQuery(request, r => {

            var query = new Query(r.limit, r.page);
            query.setSortList(r.sort);

            if (r.queries.ContainsKey("name") == true)
                query.setFilter("name","ILIKE",$"%{r.queries["name"]}%");

            return query;

        });
    }

    public static Query Category(QueriesRequest? request) {
        return QTOHelper.getQuery(request, r => {

            var query = new Query(r.limit, r.page);
            query.setSortList(r.sort);

            if (r.queries.ContainsKey("name") == true)
                query.setFilter("name","ILIKE",$"%{r.queries["name"]}%");

            return query;

        });
    }

    public static Query MonthlyService(QueriesRequest? request) {
        return QTOHelper.getQuery(request, r => {

            var query = new Query(r.limit, r.page);
            query.setSortList(r.sort);

            if (r.queries.ContainsKey("name") == true)
                query.setFilter("name","ILIKE",$"%{r.queries["name"]}%");

            if (r.queries.ContainsKey("active") == true)
                query.setFilter("isActive","=", Convert.ToBoolean(r.queries["active"]!));

            return query;

        });
    }

    public static Query EntryTags(QueriesRequest? request, long entryID) {
        return QTOHelper.getQuery(request, r => {

            var query = new Query(r.limit, r.page);
            query.setSortList(r.sort);
            query.setFilter("entryId","=",entryID);

            return query;

        });
    }

}