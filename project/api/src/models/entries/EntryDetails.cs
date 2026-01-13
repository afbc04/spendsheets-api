public sealed class EntryDetails {

    // Entry Details
    public long ID { get; set; }
    public Category? category { get; set; }
    public bool is_visible { get; set; }
    public EntryType type { get; set; }
    public int money { get; set; }
    public int? money_left { get; set; }
    public DateOnly date { get; set; }
    public DateTime last_change_date { get; set; }
    public DateOnly creation_date { get; set; }
    public DateOnly? finish_date { get; set; }
    public string? description { get; set; }
    public EntryStatus status { get; set; }

    public long tags_count { get; set; }
    public long notes_count { get; set; }
    public long movements_count { get; set; }

    // Deleted Entry Details
    public DateOnly? deletion_date { get; set; }
    public DeletedEntryStatus? deleted_status { get; set; }
    public EntryStatus? last_status { get; set; }

    // Commitment Entry Details
    public MonthlyServiceSimple? monthly_service { get; set; }
    public bool is_generated_by_system { get; set; }
    public DateOnly? scheduled_due_date { get; set; }
    public DateOnly? real_due_date { get; set; }

    public EntryDetails(
        long ID,
        Category? category,
        bool is_visible,
        EntryType type,
        int money,
        int? money_left,
        DateOnly date,
        DateTime last_change_date,
        DateOnly creation_date,
        DateOnly? finish_date,
        string? description,
        EntryStatus status,
        DateOnly? deletion_date,
        DeletedEntryStatus? deleted_status,
        EntryStatus? last_status,
        MonthlyServiceSimple? monthly_service,
        bool is_generated_by_system,
        DateOnly? scheduled_due_date,
        DateOnly? real_due_date,
        long tags_count,
        long notes_count,
        long movements_count
    )
    {
        this.ID = ID;
        this.category = category;
        this.is_visible = is_visible;
        this.type = type;
        this.money = money;
        this.money_left = money_left;
        this.date = date;
        this.last_change_date = last_change_date;
        this.creation_date = creation_date;
        this.finish_date = finish_date;
        this.description = description;
        this.status = status;
        this.deletion_date = deletion_date;
        this.deleted_status = deleted_status;
        this.last_status = last_status;
        this.monthly_service = monthly_service;
        this.is_generated_by_system = is_generated_by_system;
        this.scheduled_due_date = scheduled_due_date;
        this.real_due_date = real_due_date;
        this.tags_count = tags_count;
        this.notes_count = notes_count;
        this.movements_count = movements_count;
    }

    public EntryDetails(EntryTransaction entry, Category? category, long tags_count, long notes_count) {
        this.ID = entry.ID;
        this.category = category;
        this.is_visible = entry.is_visible;
        this.type = EntryType.Transaction;
        this.money = entry.money;
        this.money_left = null;
        this.date = entry.date;
        this.last_change_date = entry.last_change_date;
        this.creation_date = entry.creation_date;
        this.finish_date = entry.finish_date;
        this.description = entry.description;
        this.status = entry.status;

        this.deletion_date = entry.deleted_entry?.deleted_date;
        this.deleted_status = entry.deleted_entry?.status;
        this.last_status = entry.deleted_entry?.last_status;

        this.monthly_service = null;
        this.is_generated_by_system = false;
        this.scheduled_due_date = null;
        this.real_due_date = null;

        this.tags_count = tags_count;
        this.notes_count = notes_count;
        this.movements_count = 0;
    }

    private decimal? _get_money_left() {

        if (this.type != EntryType.Commitment)
            return null;
        else
            return this.money_left != null ? Money.Format((int) this.money_left) : null;

    }

    private decimal? _get_money_saved() {

        if (this.type != EntryType.Saving)
            return null;
        else
            return this.money_left != null ? Money.Format((int) this.money_left) : null;

    }

    private Dictionary<string, object?>? _get_tags() {
        return new Dictionary<string, object?>
            {
                ["count"] = this.tags_count,
                ["endpoint"] = $"/v1.0/entryTags/{this.ID}",
                ["method"] = "GET"
            };
    }

    private Dictionary<string, object?>? _get_notes() {
        return new Dictionary<string, object?>
            {
                ["count"] = this.notes_count,
                ["endpoint"] = $"/v1.0/entryNotes/{this.ID}",
                ["method"] = "GET"
            };
    }

    private Dictionary<string, object?>? _get_movements() {

        if (this.type != EntryType.Commitment)
            return null;
        else
            return new Dictionary<string, object?>
            {
                ["count"] = this.movements_count,
                ["endpoint"] = $"/v1.0/entryMovements/{this.ID}",
                ["method"] = "GET"
            };
    }

    public IDictionary<string, object?> ToJson() =>
        new Dictionary<string, object?>
        {
            ["id"] = this.ID,
            ["category"] = this.category?.to_json(),

            ["monthlyService"] = monthly_service != null ? new Dictionary<string, object?>
            {
                ["id"] = monthly_service.ID,
                ["name"] = monthly_service.name,
                ["description"] = monthly_service.description,
                ["active"] = monthly_service.active
            } : null,

            ["notes"] = _get_notes(),
            ["tags"] = _get_tags(),
            ["movements"] = _get_movements(),
            ["visible"] = this.is_visible,
            ["generatedBySystem"] = this.is_generated_by_system,
            ["type"] = EntryTypeHandler.Get(this.type),
            ["date"] = this.date,
            ["description"] = this.description,
            ["money"] = Money.Format(this.money),
            ["moneyRemaining"] = _get_money_left(),
            ["moneySaved"] = _get_money_saved(),
            ["lastChangeDate"] = Utils.convert_to_datetime(this.last_change_date),
            ["creationDate"] = this.creation_date,
            ["finishDate"] = this.finish_date,
            ["scheduledDueDate"] = this.scheduled_due_date,
            ["realDueDate"] = this.real_due_date,
            ["draft"] = this.status == EntryStatus.Draft,
            ["finished"] = this.status == EntryStatus.Done,
            ["completed"] = null,
            ["pending"] = null,
            ["deleted"] = this.status == EntryStatus.Deleted,
            ["deletedDate"] = this.deletion_date,
            ["status"] = EntryStatusHandler.Get(this.date,this.scheduled_due_date,this.status,this.deleted_status,this.money,this.money_left)
        };

}
