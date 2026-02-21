public sealed class EntryList {

    public long ID { get; set; }
    
    public long? category_ID { get; set; }
    public string? category_name { get; set; }

    public long? collection_ID { get; set; }
    public string? collection_name { get; set; }
    public bool? collection_is_monthly_service { get; set; }

    public bool is_visible { get; set; }
    public bool is_public { get; set; }
    public bool is_active { get; set; }
    public EntryType type { get; set; }
    public int money { get; set; }
    public int? money_spent { get; set; }
    public DateOnly date { get; set; }
    public DateTime last_change_date { get; set; }
    public DateOnly? due_date { get; set; }
    public EntryStatus status { get; set; }

    public EntryList(
        long ID,
        long? category_ID,
        string? category_name,
        long? collection_ID,
        string? collection_name,
        bool? collection_is_monthly_service,
        bool is_visible,
        bool is_public,
        bool is_active,
        EntryType type,
        int money,
        int? money_spent,
        DateOnly date,
        DateTime last_change_date,
        DateOnly? due_date,
        EntryStatus status
    )
    {
        this.ID = ID;

        this.category_ID = category_ID;
        this.category_name = category_name;

        this.collection_ID = collection_ID;
        this.collection_name = collection_name;
        this.collection_is_monthly_service = collection_is_monthly_service;

        this.is_visible = is_visible;
        this.is_public = is_public;
        this.is_active = is_active;
        this.type = type;
        this.money = money;
        this.money_spent = money_spent;
        this.date = date;
        this.last_change_date = last_change_date;
        this.due_date = due_date;
        this.status = status;
    }

    /*
    private decimal? _get_actual_money() {
        return this.type == EntryType.Transaction ? Money.Format(this.money) : this.money_spent == null ? null : Money.Format((long) this.money_spent);
    }

    private decimal? _get_target_money() {
        return this.type == EntryType.Transaction ? null : Money.Format(this.money);
    }

    private decimal? _get_remaining_money() {
        return this.type == EntryType.Transaction ? null : money_spent != null ? Money.Format((long) (this.money - this.money_spent)) : null;
    }

    private Dictionary<string, object?>? _get_category() {
        return new Dictionary<string, object?> {
            ["id"] = this.category!.ID,
            ["name"] = this.category!.name
        };
    }

    private Dictionary<string, object?>? _get_monthly_service() {
        return new Dictionary<string, object?> {
            ["id"] = this.monthly_service!.ID,
            ["name"] = this.monthly_service!.name
        };
    }

    public IDictionary<string, object?> ToJson() =>
        new Dictionary<string, object?> {
            ["id"] = this.ID,
            ["category"] = this.category != null ? _get_category() : null,
            ["monthlyService"] = this.monthly_service != null ? _get_monthly_service() : null,
            ["type"] = EntryTypeHandler.Get(this.type),
            ["date"] = this.date,
            ["dueDate"] = this.due_date,
            ["daysLeft"] = this.due_date != null ? (this.due_date.Value.ToDateTime(TimeOnly.MinValue) - this.date.ToDateTime(TimeOnly.MinValue)).Days : null,
            ["targetMoney"] = _get_target_money(),
            ["actualMoney"] = _get_actual_money(),
            ["remainingMoney"] = _get_remaining_money(),
            ["status"] = EntryStatusHandler.Get(this.date,this.due_date,this.status,this.deleted_status,this.money,this.money_spent)
        };*/

}