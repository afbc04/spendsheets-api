public class MonthlyServiceList {

    public long ID {set; get;}
    public string name {set; get;}
    public bool is_active {set; get;}

    public MonthlyServiceList(long ID, string name, bool active) {
        this.ID = ID;
        this.name = name;
        this.is_active = active;
    }

    public IDictionary<string,object?> to_json() {
        return new Dictionary<string,object?> {
            ["id"] = this.ID,
            ["name"] = this.name,
            ["active"] = this.is_active
        };
    }

}