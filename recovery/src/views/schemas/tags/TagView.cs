public static class TagView
{
    public static Dictionary<string,object?> ToView(Tag tag)
        => ViewifyShow(tag.ID, tag.Name, tag.Description);

    public static Dictionary<string,object?> ToView(TagModel tag, bool isHidden)
        => isHidden
            ? ViewifyHide(tag.ID)
            : ViewifyShow(tag.ID, tag.Name, tag.Description);

    private static Dictionary<string,object?> ViewifyHide(long ID)
    {
        return new Dictionary<string,object?>(){
            ["id"] = ID,
            ["name"] = "???",
            ["description"] = null,
            ["hidden"] = true
        };
    }

    private static Dictionary<string,object?> ViewifyShow(long ID, string name, string? description)
    {
        return new Dictionary<string,object?>(){
            ["id"] = ID,
            ["name"] = name,
            ["description"] = description,
            ["hidden"] = false
        };
    }

}