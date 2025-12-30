using Pages;
namespace Queries;

public class QueryUser : IQuery {

    private Querieable query {get; set;}

    public QueryUser(Querieable query) {
        this.query = query;
    }
    
    public string get_sql_listing() {
        return query.page!.get_sql_listing();
    }

    public string get_sql_filtering() {

        List<string> filters = new();

        if (query.queries.ContainsKey("level")) {

            string user_level = (string) query.queries["level"]!;
            user_level = user_level.ToLower();

            char level = user_level switch {
                "administrator" => 'A',
                "driver" => 'D',
                _ => 'T'
            };

            filters.Add($"level = '{level}'");
        }

        if (query.queries.ContainsKey("countryCode"))
            filters.Add($"countryCode = '{query.queries["countryCode"]}'");

        if (query.queries.ContainsKey("active")) {

            string active_status = Convert.ToBoolean(query.queries["active"]!) ? "IS NULL" : "IS NOT NULL";
            filters.Add($"inactiveDate {active_status}");

        }
        
        if (query.queries.ContainsKey("public"))
            filters.Add($"public = {query.queries["public"]}");
        
        if (query.queries.ContainsKey("prefixName"))
            filters.Add($"name ILIKE U&'{query.queries["prefixName"]}%'");
        
        if (filters.Count() > 0)
            return $" WHERE {string.Join(" AND ",filters)}";
        else
            return "";

    }

    public IList<string> validate() {

        var error_list = new List<string>();

        if (query.queries.ContainsKey("level")) {

            string user_level = (string) query.queries["level"]!;
            user_level = user_level.ToLower();

            if (user_level != "administrator" &&
                user_level != "driver" &&
                user_level != "traveller")
                error_list.Add("Invalid user level in query parameter");

        }

        return error_list;

    }

    public PageInput get_page() {
        return this.query.page!;
    }

}