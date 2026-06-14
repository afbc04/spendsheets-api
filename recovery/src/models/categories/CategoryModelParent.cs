public class CategoryModelParent(long ID, string name, bool hasParent)
{
    public long ID { set; get; } = ID;
    public string Name { set; get; } = name;
    public bool HasParent { set; get; } = hasParent;
}
