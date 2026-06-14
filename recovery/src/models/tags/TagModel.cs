public class TagModel(long ID, string name, string? description)
{
    public long ID { set; get; } = ID;
    public string Name { set; get; } = name;
    public string? Description { set; get; } = description;
}
