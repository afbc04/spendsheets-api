public class CollectionList {

    public long ID {set; get;}
    public string name {set; get;}
    public bool is_monthly_service {set; get;}
    public bool? is_monthly_service_active {set; get;}

    public CollectionList(long ID, string name, bool is_monthly_service, bool? is_monthly_service_active) {
        this.ID = ID;
        this.name = name;
        this.is_monthly_service = is_monthly_service;
        this.is_monthly_service_active = is_monthly_service_active;
    }

}