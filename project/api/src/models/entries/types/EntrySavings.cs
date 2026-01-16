//public class EntrySavings : Entry {

    /*
    public EntrySavings(long ID) : base(ID, EntryType.Saving) {
        this.money_spent = this.money;
    }

    public EntrySavings(long ID, bool is_visible, int money) 
        : base(ID,is_visible,EntryType.Saving,money) {
        this.money_spent = money;
    }

    public EntrySavings(long ID,long? category_id, bool is_visible, int money, int money_left, DateOnly date, DateTime last_change_date, DateOnly creation_date, DateOnly? finish_date, string? description, EntryStatus status, DeletedEntry? deleted_entry) 
        : base(ID,category_id,is_visible,EntryType.Saving,money,money_left,last_change_date,creation_date,date,finish_date,description,status,deleted_entry) {}

    public EntrySavings(EntrySavings entry) : this(
        entry.ID,
        entry.category_ID,
        entry.is_visible,
        entry.money,
        (int) entry.money_spent!,
        entry.date,
        entry.last_change_date,
        entry.creation_date,
        entry.finish_date,
        entry.description,
        entry.status,
        entry.money_spent
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

    public override void setStatusStalled() {
        this._undoDelete();
        this._undoFinish();
        this.status = EntryStatus.Stalled;
    }

    public override void setStatusAccomplished() {
        this._undoDelete();
        this._undoFinish();
        this.status = EntryStatus.Accomplished;
    }

    public override void setStatusDeleted() {
        this._undoFinish();
        this._doDelete(DeletedEntryStatus.Deleted);
        this.status = EntryStatus.Deleted;
    }

    public override void setStatusCancelled() {

        if (this.money_spent == 0)
            throw new EntryException("Savings entry can only be cancelled if some money is saved");
        
        else if (this.status == EntryStatus.OnGoing) {
            this._undoFinish();
            this._doDelete(DeletedEntryStatus.Cancelled);
            this.status = EntryStatus.Deleted;
        }
        else
            throw new EntryException("Savings entry can only be cancelled if its status is 'pending'");

    }

    public override void setStatusIgnored() =>
        throw new EntryException("Savings entry does not have this status");

    */
//}