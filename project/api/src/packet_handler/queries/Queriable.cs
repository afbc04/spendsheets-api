using Pages;

namespace Queries {

    public class Querieable {

        public PageInput? page {get; private set;}
        public IDictionary<string,object?> queries {get; private set;}

        public Querieable(PageInput? page, IDictionary<string,object?> queries) {
            this.page = page;
            this.queries = queries;
        }

    }

}