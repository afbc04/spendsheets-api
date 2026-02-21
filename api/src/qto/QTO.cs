using Queries;

public class QTO {

    public static Query Category(bool can_read, QueriesRequest? request) {
        return QTOHelper.getQuery(request, r => {

            var query = new Query(r.limit, r.page);
            query.setSortList(can_read ? r.sort : [.. r.sort.Where(s => s.is_hidden == false)]);

            if (can_read) {

                if (r.queries.ContainsKey("name") == true)
                    query.setFilter("name","ILIKE",$"%{r.queries["name"]}%");

            }

            return query;

        });
    }

    public static Query Collection(bool can_read, QueriesRequest? request) {
        return QTOHelper.getQuery(request, r => {

            var query = new Query(r.limit, r.page);
            query.setSortList(can_read ? r.sort : [.. r.sort.Where(s => s.is_hidden == false)]);

            if (r.queries.ContainsKey("name") == true)
                query.setFilter("name","ILIKE",$"%{r.queries["name"]}%");

            if (r.queries.ContainsKey("active") == true)
                query.setFilter("isMonthlyServiceActive","=", Convert.ToBoolean(r.queries["active"]!));

            if (r.queries.ContainsKey("monthlyService") == true)
                query.setFilter("isMonthlyService","=", Convert.ToBoolean(r.queries["monthlyService"]!));

            return query;

        });
    }

    public static Query EntryList(QueriesRequest? request) {
        return QTOHelper.getQuery(request, r => {

            var query = new Query(r.limit, r.page);
            query.setSortList(r.sort);

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

    public static Query EntryNotes(QueriesRequest? request, long entryID) {
        return QTOHelper.getQuery(request, r => {

            var query = new Query(r.limit, r.page);
            query.setSortList(r.sort);
            query.setFilter("entryId","=",entryID);

            return query;

        });
    }

    public static Query EntryMovements(QueriesRequest? request, long entryID) {
        return QTOHelper.getQuery(request, r => {

            var query = new Query(r.limit, r.page);
            query.setSortList(r.sort);
            query.setFilter("entryId","=",entryID);

            return query;

        });
    }

}