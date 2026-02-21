public struct QueryOrderItem{

    public string value { get; }
    public bool is_asc { get; }
    public bool is_case_insensitive { get; }
    public bool is_hidden { get; }

    public QueryOrderItem(string value, bool is_asc, bool is_case_insensitive, bool is_hidden) {
        this.value = value;
        this.is_asc = is_asc;
        this.is_case_insensitive = is_case_insensitive;
        this.is_hidden = is_hidden;
    }

}