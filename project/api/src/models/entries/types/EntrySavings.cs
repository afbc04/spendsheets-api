public class EntrySavings : Entry {

    public EntrySavings(long ID) : base(ID, EntryType.Saving) {
        this.money_spent = 0;
    }

    public EntrySavings(long ID, bool is_visible, int money) 
        : base(ID,is_visible,EntryType.Saving,money) {
        this.money_spent = money;
    }

    public EntrySavings(long ID,long? category_id, bool is_visible, int money, int? money_saved, DateOnly date, DateTime last_change_date, DateOnly creation_date, DateOnly? finish_date, DateOnly? due_date, string? description, EntryStatus status, DeletedEntryState? deleted_entry_state) 
        : base(ID,category_id,null,is_visible,EntryType.Saving,money,money_saved,last_change_date,creation_date,date,finish_date,due_date,description,status,deleted_entry_state) {}

    public EntrySavings(EntrySavings entry) : this(
        entry.ID,
        entry.category_ID,
        entry.is_visible,
        entry.money,
        entry.money_spent,
        entry.date,
        entry.last_change_date,
        entry.creation_date,
        entry.finish_date,
        entry.due_date,
        entry.description,
        entry.status,
        entry.deleted_entry_state
    ) {}

    private void _undoFinishSavings() {

        if (this.finish_date != null) {
            this.money *= -1;
            this.money_spent = 0;
        }

        this._undoFinish();

    }

    public override void setStatusDraft() {
        this._undoFinishSavings();
        this._undoDelete();
        this.status = EntryStatus.Draft;
    }

    public override void setStatusOnGoing() {
        this._undoFinishSavings();
        this._undoDelete();
        this.status = EntryStatus.OnGoing;
    }

    public override void setStatusDone() {
        this._undoDelete();
        this._doFinish();
        this.status = EntryStatus.Done;

        this.money *= -1;
        this.money_spent = null;
    }

    public override void setStatusStalled() {
        this._undoDelete();
        this._undoFinishSavings();
        this.status = EntryStatus.Stalled;
        this.money_spent = null;
    }

    public override void setStatusAccomplished() {
        this._undoDelete();
        this._undoFinishSavings();
        this.status = EntryStatus.Accomplished;
    }

    public override void setStatusDeleted() {
        this._undoFinishSavings();
        this._doDelete(DeletedEntryStatus.Deleted);
        this.status = EntryStatus.Deleted;
    }

    public override void setStatusCancelled() {
        this._undoFinishSavings();
        this._doDelete(DeletedEntryStatus.Cancelled);
        this.status = EntryStatus.Deleted;
        this.money_spent = null;
    }

    public override void setStatusIgnored() =>
        throw new EntryException("Savings entry does not have this status");
    
}