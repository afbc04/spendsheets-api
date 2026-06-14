
public class QueryPage
{
    public long Page { set; get; }
    public long Limit { set; get; }
    public List<QueryPageOrderItem> Sort { get; private set; }

    public QueryPage(long page, long limit, List<QueryPageOrderItem> sort)
    {
        this.Page = page;
        this.Limit = limit;
        this.Sort = sort;
    }
}

public static class PageRules
{

    public static readonly int PageDefault = 1;

    public static readonly int LimitMin = 2;
    public static readonly int LimitDefault = 10;
    public static readonly int LimitMax = 50;

}

public struct QueryPageOrderItem
{
    public string Value { get; }
    public bool IsAsc { get; }
    public bool is_case_insensitive { get; }
    public bool is_hidden { get; }

    public QueryPageOrderItem(string value, bool isAsc) {
        this.Value = value;
        this.IsAsc = isAsc;
    }
}