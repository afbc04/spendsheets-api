public class Entry {

    public long ID {set; get;}

    public long? category_ID {set; get;}
    public long? collection_ID {set; get;}

    public bool is_visible {set; get;}
    public bool is_public {set; get;}
    public EntryType type {set; get;}
    public bool active {set; get;}
    public DateOnly date {set; get;}
    public string? description {set; get;}

    public int money {set; get;}
    public int? money_spent {set; get;}
    
    public DateTime last_change_date {set; get;}
    public DateOnly creation_date {set; get;}
    public DateOnly? finish_date {set; get;}
    public DateOnly? due_date {set; get;}

    public EntryStatus status {set; get;}
    public DateOnly? deleted_date {set; get;}
    public EntryStatus? last_status {set; get;}
    
    public bool is_deleted => this.deleted_date != null;

    public Entry(long ID, EntryType type) {

        DateOnly current_date = DateOnly.FromDateTime(DateTime.Today);

        this.ID = ID;

        this.category_ID = null;
        this.collection_ID = null;

        this.is_visible = true;
        this.is_public = false;
        this.type = type;
        this.active = true;
        this.date = current_date;
        this.description = null;
        this.money = 0;
        this.money_spent = null;
        
        this.last_change_date = DateTime.UtcNow;
        this.creation_date = current_date;
        this.date = current_date;
        this.finish_date = null;
        this.due_date = null;

        this.status = EntryStatus.Draft;
        this.last_status = null;
        this.deleted_date = null;

    }

    public Entry(long ID, long? category_id, long? collection_id, bool is_visible, bool is_public, bool is_active, EntryType type, int money, int? money_spent, DateOnly date, DateTime last_change_date, DateOnly creation_date, DateOnly? finish_date, DateOnly? due_date, string? description, EntryStatus status, EntryStatus? last_status, DateOnly? deletion_date) {

        this.ID = ID;
        this.description = description;
        this.money = money;
        this.money_spent = money_spent;
        this.is_visible = is_visible;
        this.is_public = is_public;
        this.active = is_active;
        this.category_ID = category_id;
        this.collection_ID = collection_id;
        this.last_change_date = last_change_date;
        this.creation_date = creation_date;
        this.date = date;
        this.finish_date = finish_date;
        this.due_date = due_date;
        this.status = status;
        this.type = type;
        this.last_status = last_status;
        this.deleted_date = deletion_date;

    }

    public void _undoFinish() {
        this.finish_date = null;
    }

    public void _doFinish() {
        this.finish_date = DateOnly.FromDateTime(DateTime.Today);
    }

    public void _undoDelete() {
        this.last_status = null;
        this.deleted_date = null;
    }

    public void _doDelete(EntryStatus status) {

        this.last_status = EntryStatusHandler.IsDeleted(this.status) ? this.last_status : this.status;        
        this.deleted_date = DateOnly.FromDateTime(DateTime.Today);

    }

    public void recoverEntry() {

        if (EntryStatusHandler.IsDeleted(this.status)) {

            this.status = this.last_status ?? EntryStatus.Draft;
            
            if (this.status == EntryStatus.Done)
                this._doFinish();
            else
                this._undoFinish();

            this.last_status = null;
            this.deleted_date = null;

        }

    }

    public void undraftEntry() {

        if (this.money_spent == null)
            setStatusDone();
        else
            setStatusOnGoing();

    }

    public void addMoneyDelta(int money_delta) {

        if (this.money_spent == null) {
            this.money += money_delta;
        }
        else {
            this.money_spent += money_delta;
        }

        _updateStatus();
        this.last_change_date = DateTime.UtcNow;

    }

    private void _updateStatus() {

        if (this.money_spent != null && EntryStatusHandler.IsOngoing(this.status)) {

            if (this.money_spent == 0)
                this.status = EntryStatus.Pending;
            else if (Math.Abs((int) this.money_spent) < Math.Abs(this.money))
                this.status = EntryStatus.OnGoing;
            else
                this.status = EntryStatus.Completed;

        }

    }
    
    public void setStatus(string status) {

        status = status.ToLower();
        var obtained_status = EntryStatusHandler.Extract(status);

        switch (obtained_status) {

            case EntryStatus.Draft:
                setStatusDraft();
                break;

            case EntryStatus.Pending:
            case EntryStatus.OnGoing:
            case EntryStatus.Completed:
                setStatusOnGoing();
                break;

            case EntryStatus.Done:
                setStatusDone();
                break;

            case EntryStatus.Stalled:
                setStatusStalled();
                break;

            case EntryStatus.Deleted:
                setStatusDeleted();
                break;

            case EntryStatus.Cancelled:
                setStatusCancelled();
                break;

            case EntryStatus.Ignored:
                setStatusIgnored();
                break;

            default:
                throw new EntryException("Invalid status provided");

        }

    }

    public void setStatusDraft() {
        this._undoFinish();
        this._undoDelete();
        this.status = EntryStatus.Draft;
    }

    public void setStatusOnGoing() {
        this._undoFinish();
        this._undoDelete();

        if (this.money_spent == null)
            throw new EntryException("Entry can only have on going status if they contain target money");
        else {

            if (this.money_spent == 0)
                this.status = EntryStatus.Pending;
            else if (Math.Abs((int) this.money_spent) < Math.Abs(this.money))
                this.status = EntryStatus.OnGoing;
            else
                this.status = EntryStatus.Completed;

        }

    }

    public void setStatusDone() {
        this._undoDelete();
        this._doFinish();
        this.status = EntryStatus.Done;
    }

    public void setStatusStalled() {
        this._undoDelete();
        this._undoFinish();
        this.status = EntryStatus.Stalled;
    }

    public void setStatusDeleted() {
        this._undoFinish();
        this._doDelete(EntryStatus.Deleted);
        this.status = EntryStatus.Deleted;
    }

    public void setStatusCancelled() {

        if (EntryStatusHandler.IsOngoing(this.status)) {

            if (this.money_spent == 0)
                throw new EntryException("Entries can only be cancelled if has any actual money attached to it");
            
            else {

                this._undoFinish();
                this._doDelete(EntryStatus.Cancelled);
                this.status = EntryStatus.Cancelled;

            }
        }
        else
            throw new EntryException("Entries can only be cancelled if entry is on going");

    }

    public void setStatusIgnored() {

        if (EntryStatusHandler.IsOngoing(this.status)) {

            if (this.money_spent != 0)
                throw new EntryException("Entries can only be ignored if has no actual money attached to it");
            
            else {

                this._undoFinish();
                this._doDelete(EntryStatus.Ignored);
                this.status = EntryStatus.Ignored;

            }
        }
        else
            throw new EntryException("Entries can only be ignored if entry is on going");

    }

}