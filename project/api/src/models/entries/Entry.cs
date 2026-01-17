public abstract class Entry {

    public long ID {set; get;}
    public string? description {set; get;}
    public int money {set; get;}
    public int? money_spent {set; get;}
    public bool is_visible {set; get;}
    public long? category_ID {set; get;}
    public long? monthly_service_ID {set; get;}
    public DateTime last_change_date {set; get;}
    public DateOnly creation_date {set; get;}
    public DateOnly date {set; get;}
    public DateOnly? finish_date {set; get;}
    public DateOnly? due_date {set; get;}
    public EntryStatus status {set; get;}
    public EntryType type {set; get;}
    public DeletedEntryState? deleted_entry_state {set; get;}
    
    public bool is_deleted => this.deleted_entry_state != null;
    public bool switched_deletion_mode;

    public Entry(long ID, EntryType type) {

        DateOnly current_date = DateOnly.FromDateTime(DateTime.Today);

        this.ID = ID;
        this.description = null;
        this.money = 0;
        this.money_spent = null;
        this.is_visible = true;
        this.category_ID = null;
        this.monthly_service_ID = null;
        this.last_change_date = DateTime.UtcNow;
        this.creation_date = current_date;
        this.date = current_date;
        this.finish_date = null;
        this.due_date = null;
        this.status = EntryStatus.Draft;
        this.type = type;
        this.deleted_entry_state = null;

        this.switched_deletion_mode = false;

    }

    public Entry(long ID, bool is_visible, EntryType type, int money) {

        DateOnly current_date = DateOnly.FromDateTime(DateTime.Today);

        this.ID = ID;
        this.description = null;
        this.money = money;
        this.money_spent = null;
        this.is_visible = is_visible;
        this.category_ID = null;
        this.monthly_service_ID = null;
        this.last_change_date = DateTime.UtcNow;
        this.creation_date = current_date;
        this.date = current_date;
        this.finish_date = null;
        this.due_date = null;
        this.status = EntryStatus.Draft;
        this.type = type;
        this.deleted_entry_state = null;

        this.switched_deletion_mode = false;

    }

    public Entry(long ID, long? category_id, long? monthly_service_id, bool is_visible, EntryType type, int money, int? money_spent, DateTime last_change_date, DateOnly creation_date, DateOnly date, DateOnly? finish_date, DateOnly? due_date, string? description, EntryStatus status, DeletedEntryState? deleted_entry_state) {

        this.ID = ID;
        this.description = description;
        this.money = money;
        this.money_spent = money_spent;
        this.is_visible = is_visible;
        this.category_ID = category_id;
        this.monthly_service_ID = monthly_service_id;
        this.last_change_date = last_change_date;
        this.creation_date = creation_date;
        this.date = date;
        this.finish_date = finish_date;
        this.due_date = due_date;
        this.status = status;
        this.type = type;
        this.deleted_entry_state = deleted_entry_state;

    }

    public void _undoFinish() {
        this.finish_date = null;
    }

    public void _doFinish() {
        this.finish_date = DateOnly.FromDateTime(DateTime.Today);
    }

    public void _undoDelete() {
        this.deleted_entry_state = null;
    }

    public void _doDelete(DeletedEntryStatus status) {
        var last_status = this.deleted_entry_state == null ? this.status : this.deleted_entry_state.last_status;
        this.deleted_entry_state = new DeletedEntryState(DateOnly.FromDateTime(DateTime.Today),last_status,status);
    }

    public void recoverEntry() {

        if (this.deleted_entry_state != null) {

            this.status = this.deleted_entry_state.last_status;
            
            if (this.status == EntryStatus.Done)
                this._doFinish();
            else
                this._undoFinish();

            this.deleted_entry_state = null;

        }

    }

    public void undraftEntry() {

        if (this.type == EntryType.Transaction)
            setStatusDone();
        else
            setStatusOnGoing();

    }

    public void setStatus(string status) {

        status = status.ToLower();
        var obtained_status = EntryStatusHandler.Extract(status);
        var obtained_deleted_status = EntryStatusHandler.ExtractDelete(status);

        switch (obtained_status) {

            case EntryStatus.Draft:
                setStatusDraft();
                break;

            case EntryStatus.OnGoing:
                setStatusOnGoing();
                break;

            case EntryStatus.Done:
                setStatusDone();
                break;

            case EntryStatus.Stalled:
                setStatusStalled();
                break;

            case EntryStatus.Accomplished:
                setStatusAccomplished();
                break;

            case EntryStatus.Deleted:

                if (obtained_deleted_status == DeletedEntryStatus.Cancelled)
                    setStatusCancelled();
                else if (obtained_deleted_status == DeletedEntryStatus.Ignored)
                    setStatusIgnored();
                else
                    setStatusDeleted();

                break;

            default:
                throw new EntryException("Invalid status provided");

        }

    }

    public abstract void setStatusDraft();
    public abstract void setStatusOnGoing();
    public abstract void setStatusDone();
    public abstract void setStatusStalled();
    public abstract void setStatusAccomplished();
    public abstract void setStatusDeleted();
    public abstract void setStatusCancelled();
    public abstract void setStatusIgnored();

}