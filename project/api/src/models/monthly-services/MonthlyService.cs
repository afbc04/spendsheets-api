public abstract class MonthlyService {

    public long ID {set; get;}
    public string name {set; get;}
    public string? description {set; get;}
    public int? money_amount {set; get;}
    public bool active {set; get;}

    public MonthlyService(long ID, string name, string? description, int? money_amount, bool active) {

        this.ID = ID;
        this.name = name;
        this.description = description;
        this.money_amount = money_amount;
        this.active = active;

    }

    public MonthlyService(long ID, string name, bool active) {

        this.ID = ID;
        this.name = name;
        this.description = null;
        this.money_amount = null;
        this.active = active;

    }

    public MonthlyService(MonthlyService ms) {

        this.ID = ms.ID;
        this.name = ms.name;
        this.description = ms.description;
        this.money_amount = ms.money_amount;
        this.active = ms.active;

    }

}