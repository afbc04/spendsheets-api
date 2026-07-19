public class Workspace
{
    private long _id;
    private string _name;
    private string? _description;
    private long _initial_money;

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
                throw new SchemaException($"Name can not be empty");

            if (value.Length > WorkspaceRules.NameLengthMax)
                throw new SchemaException($"Name is too long (max {WorkspaceRules.NameLengthMax})");

            _name = value;
        }
    }

    public string? Description
    {
        get => _description;

        set
        {
            if (value is not null)
            {
                if (value.Length == 0)
                    throw new SchemaException($"Description can not be empty");

                if (value.Length > WorkspaceRules.DescriptionLengthMax)
                    throw new SchemaException($"Description is too long (max {WorkspaceRules.DescriptionLengthMax})");
            }

            _description = value;
        }
    }

    public long InitialMoney
    {
        get => _initial_money;
        set => _initial_money = value;
    }

    public Workspace(
        long id,
        string name,
        string? description,
        long initialMoney)
    {
        this._id = id;
        this._name = name;
        this._description = description;
        this._initial_money = initialMoney;
    }

    public Workspace()
    {
        this._id = 0;
        this._name = "";
        this._description = null;
        this._initial_money = 0;
    }
}

public static class WorkspaceRules 
{
    public static readonly int NameLengthMax = 30;
    public static readonly int DescriptionLengthMax = 100;
}