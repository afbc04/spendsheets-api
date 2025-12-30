using Pages;
namespace Queries;

public class QueryBusPass : IQuery {

    private Querieable query {get; set;}

    public QueryBusPass(Querieable query) {
        this.query = query;
    }
    
    public string get_sql_listing() {
        return query.page!.get_sql_listing();
    }

    public string get_sql_filtering() {

        if (query.queries.ContainsKey("localityLevel"))
            return $" WHERE localityLevel >= {query.queries["localityLevel"]}";
        else
            return "";

    }

    public IList<string> validate() {
        return new List<string>();
    }

    public PageInput get_page() {
        return this.query.page!;
    }

}