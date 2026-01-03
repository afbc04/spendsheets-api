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

}