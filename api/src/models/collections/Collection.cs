public class Collection {

    public long ID {set; get;}
    public string name {set; get;}
    public string? description {set; get;}
    public bool is_monthly_service {set; get;}

    public long? category_ID {set; get;}
    public int? money_amount {set; get;}
    public bool? is_monthly_service_active {set; get;}

    public Collection(long ID, string name, string? description, int? money_amount, long? category_ID, bool is_monthly_service, bool? is_monthly_service_active) {

        this.ID = ID;
        this.name = name;
        this.description = description;
        this.category_ID = category_ID;
        this.money_amount = money_amount;
        this.is_monthly_service = is_monthly_service;
        this.is_monthly_service_active = is_monthly_service_active;

    }

    public Collection(long ID, string name) {

        this.ID = ID;
        this.name = name;
        this.description = null;
        this.category_ID = null;
        this.money_amount = null;
        this.is_monthly_service = false;
        this.is_monthly_service_active = null;

    }

    public Collection(Collection c) {

        this.ID = c.ID;
        this.name = c.name;
        this.description = c.description;
        this.category_ID = c.category_ID;
        this.money_amount = c.money_amount;
        this.is_monthly_service = c.is_monthly_service;
        this.is_monthly_service_active = c.is_monthly_service_active;

    }

}