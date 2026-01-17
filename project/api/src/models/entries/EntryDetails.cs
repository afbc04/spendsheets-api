public sealed class EntryDetails {

    // Entry Details
    public long ID { get; set; }
    public Category? category { get; set; }
    public MonthlyServiceSimple? monthly_service { get; set; }
    public bool is_visible { get; set; }
    public EntryType type { get; set; }
    public int money { get; set; }
    public int? money_spent { get; set; }
    public int money_spent_movements { get; set; }
    public DateOnly date { get; set; }
    public DateTime last_change_date { get; set; }
    public DateOnly creation_date { get; set; }
    public DateOnly? finish_date { get; set; }
    public DateOnly? due_date { get; set; }
    public string? description { get; set; }
    public EntryStatus status { get; set; }

    // Deleted Entry Details
    public DateOnly? deletion_date { get; set; }
    public DeletedEntryStatus? deleted_status { get; set; }
    public EntryStatus? last_status { get; set; }

    public EntryDetails(
        long ID,
        Category? category,
        MonthlyServiceSimple? monthly_service,
        bool is_visible,
        EntryType type,
        int money,
        int? money_spent,
        int money_spent_movements,
        DateOnly date,
        DateTime last_change_date,
        DateOnly creation_date,
        DateOnly? finish_date,
        DateOnly? due_date,
        string? description,
        EntryStatus status,
        DateOnly? deletion_date,
        DeletedEntryStatus? deleted_status,
        EntryStatus? last_status
    )
    {
        this.ID = ID;
        this.category = category;
        this.monthly_service = monthly_service;
        this.is_visible = is_visible;
        this.type = type;
        this.money = money;
        this.money_spent = money_spent;
        this.money_spent_movements = money_spent_movements;
        this.date = date;
        this.last_change_date = last_change_date;
        this.creation_date = creation_date;
        this.finish_date = finish_date;
        this.due_date = due_date;
        this.description = description;
        this.status = status;
        this.deletion_date = deletion_date;
        this.deleted_status = deleted_status;
        this.last_status = last_status;
    }

    public EntryDetails(EntryTransaction entry, Category? category, MonthlyServiceSimple? monthly_service) {
        this.ID = entry.ID;
        this.category = category;
        this.monthly_service = monthly_service;
        this.is_visible = entry.is_visible;
        this.type = EntryType.Transaction;
        this.money = entry.money;
        this.money_spent = null;
        this.money_spent_movements = 0;
        this.date = entry.date;
        this.last_change_date = entry.last_change_date;
        this.creation_date = entry.creation_date;
        this.finish_date = entry.finish_date;
        this.due_date = null;
        this.description = entry.description;
        this.status = entry.status;

        this.deletion_date = entry.deleted_entry_state?.deleted_date;
        this.deleted_status = entry.deleted_entry_state?.delete_status;
        this.last_status = entry.deleted_entry_state?.last_status;
    }

    public EntryDetails(EntrySavings entry, Category? category) {
        this.ID = entry.ID;
        this.category = category;
        this.monthly_service = null;
        this.is_visible = entry.is_visible;
        this.type = EntryType.Saving;
        this.money = entry.money;
        this.money_spent = entry.money_spent;
        this.money_spent_movements = 0;
        this.date = entry.date;
        this.last_change_date = entry.last_change_date;
        this.creation_date = entry.creation_date;
        this.finish_date = entry.finish_date;
        this.due_date = entry.due_date;
        this.description = entry.description;
        this.status = entry.status;

        this.deletion_date = entry.deleted_entry_state?.deleted_date;
        this.deleted_status = entry.deleted_entry_state?.delete_status;
        this.last_status = entry.deleted_entry_state?.last_status;
    }

    public EntryDetails(EntryCommitment entry, Category? category, MonthlyServiceSimple? monthly_service) {
        this.ID = entry.ID;
        this.category = category;
        this.monthly_service = monthly_service;
        this.is_visible = entry.is_visible;
        this.type = EntryType.Commitment;
        this.money = entry.money;
        this.money_spent = null;
        this.money_spent_movements = (int) entry.money_spent!;
        this.date = entry.date;
        this.last_change_date = entry.last_change_date;
        this.creation_date = entry.creation_date;
        this.finish_date = entry.finish_date;
        this.due_date = entry.due_date;
        this.description = entry.description;
        this.status = entry.status;

        this.deletion_date = entry.deleted_entry_state?.deleted_date;
        this.deleted_status = entry.deleted_entry_state?.delete_status;
        this.last_status = entry.deleted_entry_state?.last_status;
    }

    private decimal? _get_actual_money(int money_spent) {
        return this.type == EntryType.Transaction ? Money.Format(this.money) : Money.Format(money_spent);
    }

    private decimal? _get_target_money() {
        return this.type == EntryType.Transaction ? null : Money.Format(this.money);
    }

    private decimal? _get_remaining_money(int money_spent) {
        return this.type == EntryType.Transaction ? null : Money.Format((long) (this.money - money_spent));
    }

    private Dictionary<string, object?>? _get_monthly_service() {
        return new Dictionary<string, object?> {
            ["id"] = this.monthly_service!.ID,
            ["name"] = this.monthly_service!.name,
            ["description"] = this.monthly_service!.description,
            ["active"] = this.monthly_service!.active
        };
    }

    private Dictionary<string, object?>? _get_links() {

        string url = $"/v1.0/entries/{this.ID}";

        return new Dictionary<string, object?> {
            ["self"] = url,
            ["notes"] = $"{url}/notes",
            ["tags"] = $"{url}/tags",
            ["movements"] = this.type == EntryType.Commitment ? $"{url}/movements" : null
        };
    }

    public IDictionary<string, object?> ToJson() {

        int money_spent = this.type == EntryType.Commitment ? this.money_spent_movements : this.money_spent ?? 0;

        return new Dictionary<string, object?> {
            ["id"] = this.ID,
            ["category"] = this.category?.to_json(),
            ["monthlyService"] = this.monthly_service != null ? _get_monthly_service() : null,
            ["visible"] = this.is_visible,
            ["type"] = EntryTypeHandler.Get(this.type),
            ["date"] = this.date,
            ["description"] = this.description,
            ["targetMoney"] = _get_target_money(),
            ["actualMoney"] = _get_actual_money(money_spent),
            ["remainingMoney"] = _get_remaining_money(money_spent),
            ["lastChangeDate"] = Utils.convert_to_datetime(this.last_change_date),
            ["creationDate"] = this.creation_date,
            ["finishDate"] = this.finish_date,
            ["dueDate"] = this.due_date,
            ["draft"] = this.status == EntryStatus.Draft,
            ["finished"] = this.status == EntryStatus.Done,
            ["completed"] = this.money_spent != null ? this.money_spent == this.money : null,
            ["pending"] = this.money_spent != null ? this.money_spent == 0 : null,
            ["deleted"] = this.status == EntryStatus.Deleted,
            ["deletedDate"] = this.deletion_date,
            ["status"] = EntryStatusHandler.Get(this.date,this.due_date,this.status,this.deleted_status,this.money,money_spent),
            ["_links"] = _get_links()
        };
    }

}