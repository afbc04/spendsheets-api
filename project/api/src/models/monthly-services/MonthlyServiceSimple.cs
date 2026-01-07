public class MonthlyServiceSimple : MonthlyService {

    public long? category_related {set; get;}

    public MonthlyServiceSimple(long ID, string name, string? description, int? money_amount, bool active, long? category_related) : base(ID,name,description,money_amount,active) {
        this.category_related = category_related;
    }

    public MonthlyServiceSimple(long ID, string name, bool active) : base(ID,name,active) {
        this.category_related = null;
    }

    public MonthlyServiceSimple(MonthlyService ms_base, long? category_related) : base(ms_base) {
        this.category_related = category_related;
    }

    public MonthlyServiceSimple(MonthlyServiceSimple ms_base) : base(ms_base) {
        this.category_related = ms_base.category_related;
    }

}