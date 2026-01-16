/*public class EntryCommitment : Entry {

    public long? monthly_service_id {set; get;}
    public bool is_generated_by_system {set; get;}
    public DateOnly? scheduled_due_date {set; get;}

    public EntryCommitment(long ID) : base(ID, EntryType.Commitment) {
        this.monthly_service_id = null;
        this.is_generated_by_system = false;
        this.scheduled_due_date = null;
        this.money_left = this.money;
    }

    public EntryCommitment(long ID, bool is_visible, int money) 
        : base(ID,is_visible,EntryType.Commitment,money) {

        this.monthly_service_id = null;
        this.is_generated_by_system = false;
        this.scheduled_due_date = null;
        this.money_left = money;
    }

    public EntryCommitment(long ID,long? category_id, bool is_visible, int money, int money_left, DateOnly date, DateTime last_change_date, DateOnly creation_date, DateOnly? finish_date, string? description, EntryStatus status, DeletedEntry? deleted_entry, long? monthly_service_id, bool is_generated_by_system, DateOnly? scheduled_due_date) 
        : base(ID,category_id,is_visible,EntryType.Commitment,money,money_left,last_change_date,creation_date,date,finish_date,description,status,deleted_entry) {
            
            this.monthly_service_id = monthly_service_id;
            this.is_generated_by_system = is_generated_by_system;
            this.scheduled_due_date = scheduled_due_date; 
        }

    public EntryCommitment(EntryCommitment entry) : this(
        entry.ID,
        entry.category_ID,
        entry.is_visible,
        entry.money,
        (int) entry.money_left!,
        entry.date,
        entry.last_change_date,
        entry.creation_date,
        entry.finish_date,
        entry.description,
        entry.status,
        entry.deleted_entry,
        entry.monthly_service_id,
        entry.is_generated_by_system,
        entry.scheduled_due_date
    ) {}

    public override void setStatusDraft() {
        this._undoFinish();
        this._undoDelete();
        this.status = EntryStatus.Draft;
    }

    public override void setStatusOnGoing() {
        this._undoFinish();
        this._undoDelete();
        this.status = EntryStatus.OnGoing;
    }

    public override void setStatusDone() {
        this._undoDelete();
        this._doFinish();
        this.status = EntryStatus.Done;
    }

    public override void setStatusStalled() =>
        throw new EntryException("Commitment entry does not have this status");

    public override void setStatusAccomplished() =>
        throw new EntryException("Commitment entry does not have this status");

    public override void setStatusDeleted() {
        this._undoFinish();
        this._doDelete(DeletedEntryStatus.Deleted);
        this.status = EntryStatus.Deleted;
    }

    public override void setStatusCancelled() {

        if (this.is_generated_by_system)
            throw new EntryException("Commitment entry can only be cancelled if its made by user");
        
        else if (this.status == EntryStatus.OnGoing) {
            this._undoFinish();
            this._doDelete(DeletedEntryStatus.Cancelled);
            this.status = EntryStatus.Deleted;
        }
        else
            throw new EntryException("Commitment entry can only be cancelled if its status is 'ongoing'");

    }

    public override void setStatusIgnored() {

        if (this.is_generated_by_system == false)
            throw new EntryException("Commitment entry can only be ignored if its made by system");
        else {
            this._undoFinish();
            this._doDelete(DeletedEntryStatus.Ignored);
            this.status = EntryStatus.Deleted;
        }

    }

}*/