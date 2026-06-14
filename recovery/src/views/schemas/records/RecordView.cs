public static class RecordView
{
    public static Dictionary<string,object?> ToView(Record record)
        => ViewifyShow(record.ID, record.Description, record.Date, record.TotalMoney, record.IsInvisible, record.IsPublic, record.CreatedAt, record.UpdatedAt, record.DeletedAt, record.Status, record.Products);

/*
    public static Dictionary<string,object?> ToView(CategoryModel category, bool isHidden)
        => isHidden
            ? ViewifyHide(category.ID, category.CreationDate, category.ParentID)
            : ViewifyShow(category.ID, category.Name, category.CreationDate, category.Description, category.ParentID, category.ParentName);
*/
    private static Dictionary<string,object?> ViewifyShow(long ID, string? description, DateOnly date, long totalMoney, bool invisible, bool isPublic, DateOnly createdAt, DateOnly updatedAt, DateOnly? deletedAt, RecordStatus status, List<RecordItem> recordItems)
    {
        return new Dictionary<string,object?>(){
            ["id"] = ID,
            ["description"] = description,
            ["date"] = date,
            ["money"] = totalMoney,
            ["invisible"] = invisible,
            ["public"] = isPublic,
            ["createdAt"] = createdAt,
            ["updatedAt"] = updatedAt,
            ["deletedAt"] = deletedAt,
            ["status"] = Record.StringifyStatus(status),
            ["draft"] = status == RecordStatus.Draft,
            ["deleted"] = status == RecordStatus.Deleted,
            ["singleProduct"] = recordItems.Count == 1,
            ["products"] = recordItems,
            ["hidden"] = false
        };
    }
/*
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
    }*/
/*
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
    }*/
}