public class Record
{
    private long _id;
    private string? _description;
    private DateOnly _date;
    private long _totalMoney;
    private bool _invisible;
    private bool _public;
    private DateOnly _createdAt;
    private DateOnly _updatedAt;
    private DateOnly? _deletedAt;
    private RecordStatus _status;
    private List<RecordItem> _products;

    public long ID
    {
        get => _id;
        set => _id = value;
    }

    public string? Description
    {
        get => _description;

        set
        {
            if (value?.Length > RecordRules.DescriptionLengthMax)
                throw new SchemaException(ErrorCategory.RECORD_DESCRIPTION_MAX);

            _description = value;
        }
    }

    public DateOnly Date
    {
        get => _date;

        set
        {
            if (value > DateOnly.FromDateTime(DateTime.UtcNow))
                throw new SchemaException(ErrorCategory.RECORD_DATE_IS_ON_FUTURE);

            _date = value;
        }
    }

    public long TotalMoney
    {
        get => _totalMoney;
    }

    public bool IsInvisible
    {
        get => _invisible;
        set => _invisible = value;
    }

    public bool IsPublic
    {
        get => _public;
        set => _public = value;
    }

    public DateOnly CreatedAt
    {
        get => _createdAt;
    }

    public DateOnly UpdatedAt
    {
        get => _updatedAt;
    }

    public void RefreshUpdateDate()
    {
        this._updatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public DateOnly? DeletedAt
    {
        get => _deletedAt;
    }

    public RecordStatus Status
    {
        get => _status;

        set
        {
            switch (value)
            {
                case RecordStatus.Draft:
                    _deletedAt = null;
                    break;

                case RecordStatus.Posted:
                    _deletedAt = null;
                    break;

                case RecordStatus.Deleted:
                    _deletedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                    break;
            }

            _status = value;
        }
    }

    public void SetStatus(string status)
    {
        switch (status.ToLower())
        {
            case "draft":
                this.Status = RecordStatus.Draft;
                break;

            case "posted":
                this.Status = RecordStatus.Posted;
                break;

            case "deleted":
                this.Status = RecordStatus.Deleted;
                break;

            default:
                throw new SchemaException(ErrorCategory.RECORD_STATUS_UNKNOWN);
        }
    }

    public static string StringifyStatus(RecordStatus status)
    {
        return status switch
        {
            RecordStatus.Draft => "draft",
            RecordStatus.Posted => "posted",
            RecordStatus.Deleted => "deleted",
            _ => "???"
        };
    }

    public List<RecordItem> Products
    {
        get => _products;
    }

    public Record(
        long id,
        string? description,
        DateOnly date,
        long totalMoney,
        bool invisible,
        bool isPublic,
        DateOnly createdAt,
        DateOnly updatedAt,
        DateOnly? deletedAt,
        RecordStatus status,
        List<RecordItem>  products)
    {
        this._id = id;
        this._description = description;
        this._date = date;
        this._totalMoney = totalMoney;
        this._invisible = invisible;
        this._public = isPublic;
        this._createdAt = createdAt;
        this._updatedAt = updatedAt;
        this._deletedAt = deletedAt;
        this._status = status;
        this._products = products;
    }

    public Record(long id = 0)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        this._id = id;
        this._description = null;
        this._date = today;
        this._totalMoney = 0;
        this._invisible = false;
        this._public = false;
        this._createdAt = today;
        this._updatedAt = today;
        this._deletedAt = null;
        this._status = RecordStatus.Posted;
        this._products = [];
    }

    public Record(long id,DateOnly createdAt)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        this._id = id;
        this._description = null;
        this._date = today;
        this._totalMoney = 0;
        this._invisible = false;
        this._public = false;
        this._createdAt = createdAt;
        this._updatedAt = today;
        this._deletedAt = null;
        this._status = RecordStatus.Posted;
        this._products = [];
    }

/*
    public Category(CategoryModelWithSubcategoryCount model)
    {
        this._id = model.ID;
        this._name = model.Name;
        this._description = model.Description;
        this._parent_id = model.ParentID;
        this._creation_date = model.CreationDate;
    }*/
}

public static class RecordRules 
{
    public static readonly int DescriptionLengthMax = 300;
    public static readonly string[] AvailableStatus = ["draft", "posted", "deleted"];
}
