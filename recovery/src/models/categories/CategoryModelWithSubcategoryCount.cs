public class CategoryModelWithSubcategoryCount(long ID, string name, string? description, long? parentId, string? parentName, DateOnly creationDate, long childsCount)
{
    public long ID { set; get; } = ID;
    public string Name { set; get; } = name;
    public string? Description { set; get; } = description;
    public long? ParentID { set; get; } = parentId;
    public string? ParentName { set; get; } = parentName;
    public DateOnly CreationDate { set; get; } = creationDate;
    public long ChildsCount { set; get; } = childsCount;
}
