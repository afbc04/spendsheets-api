public sealed class DeletedEntryState {

    public DateOnly deleted_date {set; get;}
    public EntryStatus last_status {set; get;}
    public DeletedEntryStatus delete_status {set; get;}

    public DeletedEntryState(EntryStatus last_status) {

        this.deleted_date = DateOnly.FromDateTime(DateTime.Today);
        this.last_status = last_status;
        this.delete_status = DeletedEntryStatus.Deleted;

    }

    public DeletedEntryState(DateOnly deleted_date, EntryStatus last_status, DeletedEntryStatus status) {

        this.deleted_date = deleted_date;
        this.last_status = last_status;
        this.delete_status = status;

    }

}