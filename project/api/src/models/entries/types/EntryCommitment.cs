public class EntryCommitment : Entry {

    public EntryCommitment(long ID) : base(ID, EntryType.Commitment) {
        this.money_spent = 0;
    }

    public EntryCommitment(long ID, bool is_visible, int money) 
        : base(ID,is_visible,EntryType.Commitment,money) {
        this.money_spent = 0;
    }

    public EntryCommitment(long ID,long? category_id, long? monthly_service_id, bool is_visible, int money, int money_spent, DateOnly date, DateTime last_change_date, DateOnly creation_date, DateOnly? finish_date, DateOnly? due_date, string? description, EntryStatus status, DeletedEntryState? deleted_entry_state) 
        : base(ID,category_id,monthly_service_id,is_visible,EntryType.Commitment,money,money_spent,last_change_date,creation_date,date,finish_date,due_date,description,status,deleted_entry_state) {}

    public EntryCommitment(EntryCommitment entry) : this(
        entry.ID,
        entry.category_ID,
        entry.monthly_service_ID,
        entry.is_visible,
        entry.money,
        (int) entry.money_spent!,
        entry.date,
        entry.last_change_date,
        entry.creation_date,
        entry.finish_date,
        entry.due_date,
        entry.description,
        entry.status,
        entry.deleted_entry_state
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

        if (this.money_spent == 0)
            throw new EntryException("Commitment entry can only be cancelled if has movements attached to it");

        if (this.status == EntryStatus.OnGoing) {
            this._undoFinish();
            this._doDelete(DeletedEntryStatus.Cancelled);
            this.status = EntryStatus.Deleted;
        }
        else
            throw new EntryException("Commitment entry can only be cancelled if entry is on going");

    }

    public override void setStatusIgnored() {

        if (this.money_spent != 0)
            throw new EntryException("Commitment entry can only be ignored if has no movements attached to it");
        else {
            this._undoFinish();
            this._doDelete(DeletedEntryStatus.Ignored);
            this.status = EntryStatus.Deleted;
        }

    }

}