public static class TagListView
{
    public static Dictionary<string,object?> ToView(TagModelList tag, bool isHidden)
        => isHidden
            ? ViewifyHide(tag.ID)
            : ViewifyShow(tag.ID, tag.Name);

    private static Dictionary<string,object?> ViewifyHide(long ID)
    {
        return new Dictionary<string,object?>(){
            ["id"] = ID,
            ["name"] = "???",
            ["hidden"] = true
        };
    }

    private static Dictionary<string,object?> ViewifyShow(long ID, string name)
    {
        return new Dictionary<string,object?>(){
            ["id"] = ID,
            ["name"] = name,
            ["hidden"] = false
        };
    }

}