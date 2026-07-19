public class Record
{
    private long _id;
    private string? _note;
    private long _value;
    private DateOnly _date;
    private long _workspace;
    private bool _is_public;
    private bool _invisible;
    private RecordStatus _status;
    private DateOnly _creation_date;
    private DateOnly _updated_date;
    private DateOnly? _deletion_date;

    public long ID
    {
        get => _id;
        set => _id = value;
    }

    public string? Note
    {
        get => _note;

        set
        {
            if (value is not null)
            {
                if (value.Length == 0)
                    throw new SchemaException($"Note can not be empty");

                if (value.Length > RecordRules.NoteLengthMax)
                    throw new SchemaException($"Note is too long (max {RecordRules.NoteLengthMax})");
            }

            _note = value;
        }
    }

    public long Value
    {
        get => _value;

        set
        {
            if (value == 0)
                throw new SchemaException($"Value can not be zero");

            _value = value;
        }
    }

    public DateOnly Date
    {
        get => _date;

        set
        {
            if (value > DateOnly.FromDateTime(DateTime.UtcNow))
                throw new SchemaException($"Date of record can not be after current day");

            _date = value;
        }
    }

    public long Workspace
    {
        get => _workspace;
        set => _workspace = value;
    }

    public bool Public
    {
        get => _is_public;
        set => _is_public = value;
    }

    public bool Invisible
    {
        get => _invisible;
        set => _invisible = value;
    }

    public RecordStatus Status
    {
        get => _status;

        set
        {
            if (value == _status) return;

            _deletion_date = value == RecordStatus.Deleted ? DateOnly.FromDateTime(DateTime.UtcNow) : null;
            _status = value;
        }
    }

    public DateOnly CreationDate
    {
        get => _creation_date;
    }

    public DateOnly UpdatedDate
    {
        get => _updated_date;
    }

    public DateOnly? DeletedDate
    {
        get => _deletion_date;
    }

    public void RefreshUpdateDate()
    {
        _updated_date = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public Record(
        long id,
        string? note,
        long value,
        DateOnly date,
        long workspace,
        bool isInvisible,
        bool isPublic,
        DateOnly creationDate,
        DateOnly updatedDate,
        DateOnly? deletedDate,
        RecordStatus status)
    {
        this._id = id;
        this._note = note;
        this._value = value;
        this._date = date;
        this._workspace = workspace;
        this._is_public = isPublic;
        this._invisible = isInvisible;
        this._status = status;
        this._creation_date = creationDate;
        this._updated_date = updatedDate;
        this._deletion_date = deletedDate;
    }

    public Record()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

        this._id = 0;
        this._note = null;
        this._value = 0;
        this._date = today;
        this._workspace = 0;
        this._is_public = false;
        this._invisible = false;
        this._status = RecordStatus.Draft;
        this._creation_date = today;
        this._updated_date = today;
        this._deletion_date = null;
    }
}

public static class RecordRules 
{
    public static readonly int NoteLengthMax = 100;
}

public enum RecordStatus 
{
    Draft,
    Posted,
    Deleted
}

public static class RecordStatusFormatter
{
    public static int Import(RecordStatus status)
    {
        return (int)status;
    }

    public static RecordStatus Export(int status)
    {
        return (RecordStatus)status;
    }

    public static string ToString(RecordStatus status)
    {
        return status switch
        {
            RecordStatus.Draft => "draft",
            RecordStatus.Posted => "posted",
            RecordStatus.Deleted => "deleted",
            _ => "???"
        };
    }

    public static RecordStatus? Parse(string status)
    {
        return status switch
        {
            "draft" => RecordStatus.Draft,
            "posted" => RecordStatus.Posted,
            "deleted" => RecordStatus.Deleted,
            _ => null
        };
    }
}