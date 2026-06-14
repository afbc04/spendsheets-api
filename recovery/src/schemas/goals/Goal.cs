public class Goal
{
    private long _id;
    private string _name = null!;
    private string? _description;
    private long _targetMoney;
    private long _effectiveMoney;
    private long _savedMoney;
    private bool _isLoan;
    private DateOnly _createdAt;
    private DateOnly _updatedAt;
    private DateOnly _beginDate;
    private DateOnly? _scheduledEndDate;
    private DateOnly? _realEndDate;
    private GoalStatus _status;

    public long ID
    {
        get => _id;
        set => _id = value;
    }
/*
    public string Name
    {
        get => _name;

        set
        {
            if (value.Length == 0) 
                throw new SchemaException(ErrorCategory.CATEGORY_NAME_EMPTY);

            if (value.Length > CategoryRules.NameLengthMax)
                throw new SchemaException(ErrorCategory.CATEGORY_NAME_MAX);

            _name = value;
        }
    }

    public string? Description
    {
        get => _description;

        set
        {
            if (value?.Length > CategoryRules.DescriptionLengthMax)
                throw new SchemaException(ErrorCategory.CATEGORY_DESCRIPTION_MAX);

            _description = value;
        }
    }

    public long? ParentID
    {
        get => _parent_id;
        set => _parent_id = value;
    }

    public DateOnly CreationDate
    {
        get => _creation_date;
    }

    public Category(
        long id,
        string name,
        DateOnly creationDate,
        string? description = null,
        long? parentId = null)
    {
        this._id = id;
        this._name = name;
        this._description = description;
        this._parent_id = parentId;
        this._creation_date = creationDate;
    }

    public Category(long id = 0)
    {
        this._id = id;
        this._name = "";
        this._description = null;
        this._parent_id = null;
        this._creation_date = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public Category(long ID,DateOnly creationDate)
    {
        this._id = ID;
        this._name = "";
        this._description = null;
        this._parent_id = null;
        this._creation_date = creationDate;
    }

    public Category(CategoryModelWithSubcategoryCount model)
    {
        this._id = model.ID;
        this._name = model.Name;
        this._description = model.Description;
        this._parent_id = model.ParentID;
        this._creation_date = model.CreationDate;
    }*/
}
/*
public static class CategoryRules 
{
    public static readonly int DescriptionLengthMax = 150;
    public static readonly int NameLengthMax = 40;
}*/
