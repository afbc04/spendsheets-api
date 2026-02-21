public class Query {

    private IList<string> _filter;
    private IList<QueryOrderItem> _order;
    private Dictionary<string, object?> _params = new();

    public long limit { get; private set; }
    public long offset { get; private set; }
    public long page { get; private set; }

    public Query(long limit, long page) {

        this.setLimit(limit);
        this.setPage(page);

        this._filter = new List<string>();
        this._order = new List<QueryOrderItem>();

    }

    public string? getFilter() {
        return this._filter.Count == 0 ? null : "WHERE " + string.Join(" AND ", this._filter);
    }

    public IReadOnlyDictionary<string, object?> getParameters() {
        return this._params;
    }

    public string? getSort(){

        if (_order.Count == 0)
            return null;

        return "ORDER BY " + string.Join(", ",_order.Select(s => {
                var value = s.is_case_insensitive ? $"LOWER({s.value})" : s.value;
                return $"{value} {(s.is_asc ? "ASC" : "DESC")}";
            }));

    }


    public void setFilter(string column, string op, object? value) {

        var param = $"@p{_params.Count}";
        _filter.Add($"{column} {op} {param}");
        _params[param] = value;
        
    }

    public void setSortList(IList<QueryOrderItem> order) {
        this._order = order;
    }

    public void setSort(string column, bool is_asc, bool is_case_insensitive, bool is_hidden) {
        _order.Add(new QueryOrderItem(column,is_asc,is_case_insensitive,is_hidden));
    }

    public void setLimit(long limit) {
        this.limit = limit;
    }

    public void setPage(long page) {
        this.page = page;
        this.offset = (page - 1) * this.limit;
    }

}
