public struct QueryOrderItem{

    public string value { get; }
    public bool is_asc { get; }

    public QueryOrderItem(string value, bool is_asc) {
        this.value = value;
        this.is_asc = is_asc;
    }

}