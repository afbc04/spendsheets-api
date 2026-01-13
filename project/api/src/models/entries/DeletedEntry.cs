public sealed class DeletedEntry {

    public DateOnly deleted_date {set; get;}
    public EntryStatus last_status {set; get;}
    public DeletedEntryStatus status {set; get;}

    public DeletedEntry(EntryStatus last_status) {

        this.deleted_date = DateOnly.FromDateTime(DateTime.Today);
        this.last_status = last_status;
        this.status = DeletedEntryStatus.Deleted;

    }

    public DeletedEntry(DateOnly deleted_date, EntryStatus last_status, DeletedEntryStatus status) {

        this.deleted_date = deleted_date;
        this.last_status = last_status;
        this.status = status;

    }

}