namespace Queries {

    public class QueriesRequest {

        public long page;
        public long limit;
        public IList<QueryOrderItem> sort {get; private set;}
        public IDictionary<string,object?> queries {get; private set;}

        public QueriesRequest(long? page, long? limit, IList<QueryOrderItem> sort,IDictionary<string,object?> queries) {

            this.page = page ?? PageRules.page_default;
            this.limit = limit ?? PageRules.limit_default;
            this.sort = sort;
            this.queries = queries;

        }

    }

}