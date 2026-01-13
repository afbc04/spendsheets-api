public class EntryTransaction : Entry {

    public EntryTransaction(long ID, bool is_visible, int money) 
        : base(ID,is_visible,EntryType.Transaction,money) {}

    public EntryTransaction(long ID,long? category_id, bool is_visible, int money, DateOnly date, DateTime last_change_date, DateOnly creation_date, DateOnly? finish_date, string? description, EntryStatus status, DeletedEntry? deleted_entry) 
        : base(ID,category_id,is_visible,EntryType.Transaction,money,null,last_change_date,creation_date,date,finish_date,description,status,deleted_entry) {}

    /*
    public EntryTransaction(EntryTransaction entry) {
        return new EntryTransaction
    }*/

}