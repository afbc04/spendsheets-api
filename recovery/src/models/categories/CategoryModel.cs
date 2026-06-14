public class CategoryModel(long ID, string name, string? description, long? parentId, string? parentName, DateOnly creationDate)
{
    public long ID { set; get; } = ID;
    public string Name { set; get; } = name;
    public string? Description { set; get; } = description;
    public long? ParentID { set; get; } = parentId;
    public string? ParentName { set; get; } = parentName;
    public DateOnly CreationDate { set; get; } = creationDate;
}
