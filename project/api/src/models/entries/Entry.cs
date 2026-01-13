public abstract class Entry {

    public long ID {set; get;}
    public long? category_ID {set; get;}
    public bool is_visible {set; get;}
    public EntryType type {set; get;}
    public int money {set; get;}
    public int? money_left {set; get;}
    public DateTime last_change_date {set; get;}
    public DateOnly creation_date {set; get;}
    public DateOnly date {set; get;}
    public DateOnly? finish_date {set; get;}
    public string? description {set; get;}
    public EntryStatus status {set; get;}
    public DeletedEntry? deleted_entry {set; get;}
    public bool is_deleted => this.deleted_entry != null;

    public Entry(long ID, bool is_visible, EntryType type, int money) {

        DateOnly current_date = DateOnly.FromDateTime(DateTime.Today);

        this.ID = ID;
        this.category_ID = null;
        this.is_visible = is_visible;
        this.type = type;
        this.money = money;
        this.money_left = null;
        this.last_change_date = DateTime.UtcNow;
        this.creation_date = current_date;
        this.date = current_date;
        this.finish_date = null;
        this.description = null;
        this.status = EntryStatus.Draft;
        this.deleted_entry = null;

    }

    public Entry(long ID, long? category_id, bool is_visible, EntryType type, int money, int? money_left, DateTime last_change_date, DateOnly creation_date, DateOnly date, DateOnly? finish_date, string? description, EntryStatus status, DeletedEntry? deleted_entry) {

        this.ID = ID;
        this.category_ID = category_id;
        this.is_visible = is_visible;
        this.type = type;
        this.money = money;
        this.money_left = money_left;
        this.last_change_date = last_change_date;
        this.creation_date = creation_date;
        this.date = date;
        this.finish_date = finish_date;
        this.description = description;
        this.status = status;
        this.deleted_entry = deleted_entry;

    }

    public void _undoFinish() {
        this.finish_date = null;
    }

    public void _doFinish() {
        this.finish_date = DateOnly.FromDateTime(DateTime.Today);
    }

    public void _undoDelete() {
        this.deleted_entry = null;
    }

    public void _doDelete(DeletedEntryStatus status) {
        this.deleted_entry = new DeletedEntry(DateOnly.FromDateTime(DateTime.Today),this.status,status);
    }

    public void setStatusDraft() {
        this._undoFinish();
        this._undoDelete();
        this.status = EntryStatus.Draft;
    }

    public void setStatusOnGoing() {
        this._undoFinish();
        this._undoDelete();
        this.status = EntryStatus.OnGoing;
    }

    public void setStatusDone() {
        this._undoDelete();
        this._doFinish();
        this.status = EntryStatus.Done;
    }

    public void setStatusStalled() {
        this._undoFinish();
        this._undoDelete();
        this.status = EntryStatus.Stalled;
    }

    public void setStatusAccomplished() {
        this._undoFinish();
        this._undoDelete();
        this.status = EntryStatus.Accomplished;
    }

    public void setStatusDeleted() {
        this._undoFinish();
        this._doDelete(DeletedEntryStatus.Deleted);
        this.status = EntryStatus.Deleted;
    }

    public void setStatusCancelled() {
        this._undoFinish();
        this._doDelete(DeletedEntryStatus.Cancelled);
        this.status = EntryStatus.Deleted;
    }

    public void setStatusIgnored() {
        this._undoFinish();
        this._doDelete(DeletedEntryStatus.Ignored);
        this.status = EntryStatus.Deleted;
    }

    public void recoverEntry() {
        this.status = this.deleted_entry!.last_status;
        
        if (this.status == EntryStatus.Done)
            this._doFinish();
        else
            this._undoFinish();

        this.deleted_entry = null;
    }


    /*
    public Category(long ID, string name, string? description) {

        this.ID = ID;
        this.name = name;
        this.description = description;

    }

    public IDictionary<string,object?> to_json() {
        return new Dictionary<string,object?> {
            ["id"] = this.ID,
            ["name"] = this.name,
            ["description"] = this.description
        };
    }*/


}