public class EntryTransaction : Entry {

    public EntryTransaction(long ID) : base(ID, EntryType.Transaction) {}

    public EntryTransaction(long ID, bool is_visible, int money) 
        : base(ID,is_visible,EntryType.Transaction,money) {}

    public EntryTransaction(long ID, long? category_id, long? monthly_service_id, bool is_visible, int money, DateOnly date, DateTime last_change_date, DateOnly creation_date, DateOnly? finish_date, string? description, EntryStatus status, DeletedEntryState? deleted_entry) 
        : base(ID,category_id,monthly_service_id,is_visible,EntryType.Transaction,money,null,last_change_date,creation_date,date,finish_date,null,description,status,deleted_entry) {}

    public EntryTransaction(EntryTransaction entry) : this(
        entry.ID,
        entry.category_ID,
        entry.monthly_service_ID,
        entry.is_visible,
        entry.money,
        entry.date,
        entry.last_change_date,
        entry.creation_date,
        entry.finish_date,
        entry.description,
        entry.status,
        entry.deleted_entry_state
    ) {}

    public override void setStatusDraft() {
        this._undoFinish();
        this._undoDelete();
        this.status = EntryStatus.Draft;
    }

    public override void setStatusOnGoing() =>
        throw new EntryException("Transaction entry does not have this status");

    public override void setStatusDone() {
        this._undoDelete();
        this._doFinish();
        this.status = EntryStatus.Done;
    }

    public override void setStatusStalled() =>
        throw new EntryException("Transaction entry does not have this status");

    public override void setStatusAccomplished() =>
        throw new EntryException("Transaction entry does not have this status");

    public override void setStatusDeleted() {
        this._undoFinish();
        this._doDelete(DeletedEntryStatus.Deleted);
        this.status = EntryStatus.Deleted;
    }

    public override void setStatusCancelled() =>
        throw new EntryException("Transaction entry does not have this status");

    public override void setStatusIgnored() =>
        throw new EntryException("Transaction entry does not have this status");

}