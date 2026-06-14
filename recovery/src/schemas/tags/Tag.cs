public class Tag
{
    private long _id;
    private string _name = null!;
    private string? _description;
    
    public long ID
    {
        get => _id;
        set => _id = value;
    }

    public string Name
    {
        get => _name;

        set
        {
            if (value.Length == 0) 
                throw new SchemaException(ErrorCategory.TAG_NAME_EMPTY);

            if (value.Length > TagRules.NameLengthMax)
                throw new SchemaException(ErrorCategory.TAG_NAME_MAX);

            _name = value;
        }
    }

    public string? Description
    {
        get => _description;

        set
        {
            if (value?.Length > TagRules.DescriptionLengthMax)
                throw new SchemaException(ErrorCategory.TAG_DESCRIPTION_MAX);

            _description = value;
        }
    }

    public Tag(
        long id,
        string name,
        string? description = null)
    {
        this._id = id;
        this._name = name;
        this._description = description;
    }

    public Tag(long id = 0)
    {
        this._id = id;
        this._name = "";
        this._description = null;
    }
}

public static class TagRules 
{
    public static readonly int DescriptionLengthMax = 150;
    public static readonly int NameLengthMax = 40;
}