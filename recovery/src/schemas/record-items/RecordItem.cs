public class RecordItem
{
    private long _id;
    private string? _note;
    private long _money;
    private long? _category;
    private HashSet<long> _tags;
    private HashSet<long> _goals;

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
            if (value?.Length > RecordItemRules.NoteLengthMax)
                throw new SchemaException(ErrorCategory.RECORD_ITEM_NOTE_MAX);

            _note = value;
        }
    }

    public long Money
    {
        get => _money;
        set => _money = value;
    }

    public long? CategoryId
    {
        get => _category;
        set => _category = value;
    }

    public HashSet<long> Tags
    {
        get => _tags;
        set => _tags = value;
    }

    public void RemoveTag(long tag)
    {
        this._tags.Remove(tag);
    }

    public void AddTag(long tag)
    {
        this._tags.Add(tag);
    }

    public HashSet<long> Goals
    {
        get => _goals;
        set => _goals = value;
    }

    public void RemoveGoal(long goal)
    {
        this._goals.Remove(goal);
    }

    public void AddGoal(long goal)
    {
        this._goals.Add(goal);
    }

    public RecordItem(
        long id,
        string? note,
        long money,
        long? category,
        HashSet<long> tags,
        HashSet<long> goals)
    {
        this._id = id;
        this._note = note;
        this._money = money;
        this._category = category;
        this._tags = tags;
        this._goals = goals;
    }

    public RecordItem(long id = 0)
    {
        this._id = id;
        this._note = null;
        this._money = 0;
        this._category = null;
        this._tags = [];
        this._goals = [];
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

public static class RecordItemRules 
{
    public static readonly int NoteLengthMax = 60;
}
