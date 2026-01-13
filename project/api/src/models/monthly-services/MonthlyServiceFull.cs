public class MonthlyServiceFull : MonthlyService {

    public Category? category_related {set; get;}

    public MonthlyServiceFull(long ID, string name, string? description, int? money_amount, bool active, Category? category_related) : base(ID,name,description,money_amount,active) {
        this.category_related = category_related;
    }

    public MonthlyServiceFull(MonthlyService ms_base, Category? category_related) : base(ms_base) {
        this.category_related = category_related;
    }

    public MonthlyServiceSimple convertToSimple() {
        return new MonthlyServiceSimple(this,this.category_related?.ID);
    }

    public IDictionary<string,object?> to_json() {
        return new Dictionary<string,object?> {
            ["id"] = this.ID,
            ["name"] = this.name,
            ["description"] = this.description,
            ["categoryRelated"] = this.category_related?.to_json(),
            ["moneyAmount"] = this.money_amount == null ? null : Money.Format((int) this.money_amount),
            ["active"] = this.active
        };
    }

}