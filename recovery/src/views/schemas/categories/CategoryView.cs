public static class CategoryView
{
    public static Dictionary<string,object?> ToView(Category category, CategoryModelParent? categoryParent)
        => ViewifyShow(category.ID, category.Name, category.CreationDate, category.Description, categoryParent?.ID, categoryParent?.Name);

    public static Dictionary<string,object?> ToView(CategoryModel category, bool isHidden)
        => isHidden
            ? ViewifyHide(category.ID, category.CreationDate, category.ParentID)
            : ViewifyShow(category.ID, category.Name, category.CreationDate, category.Description, category.ParentID, category.ParentName);

    private static Dictionary<string,object?> ViewifyShow(long ID, string name, DateOnly creationDate, string? description, long? parentId, string? parentName)
    {
        return new Dictionary<string,object?>(){
            ["id"] = ID,
            ["name"] = name,
            ["description"] = description,
            ["createdAt"] = creationDate,
            ["subcategory"] = parentId is not null,
            ["parent"] = ViewifyParentShow(parentId, parentName),
            ["hidden"] = false
        };
    }

    private static Dictionary<string,object?> ViewifyHide(long ID, DateOnly creationDate, long? parentId)
    {
        return new Dictionary<string,object?>(){
            ["id"] = ID,
            ["name"] = "???",
            ["description"] = null,
            ["createdAt"] = creationDate,
            ["subcategory"] = parentId is not null,
            ["parent"] = ViewifyParentHide(parentId),
            ["hidden"] = true
        };
    }

    public static Dictionary<string,object?>? ViewifyParentShow(long? parentId, string? parentName)
    {
        return parentId is null
            ? null
            : new Dictionary<string,object?>(){
                ["id"] = parentId,
                ["name"] = parentName
            };
    }

    public static Dictionary<string,object?>? ViewifyParentHide(long? parentId)
    {
        return parentId is null
            ? null
            : new Dictionary<string,object?>(){
                ["id"] = parentId,
                ["name"] = "???"
            };
    }
}