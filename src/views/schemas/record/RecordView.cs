public static class RecordView
{
    public static Dictionary<string,object?> ToView(Record record, bool hidden)
        => !hidden 
            ? ViewifyShow(record.ID, record.Note, record.Value, record.Workspace, record.Date, record.Public, record.Invisible, record.CreationDate, record.UpdatedDate, record.DeletedDate, record.Status)
            : ViewifyHide(record.ID, record.Workspace, record.Date, record.Public, record.CreationDate, record.UpdatedDate, record.DeletedDate, record.Status);

    private static Dictionary<string,object?> ViewifyShow(long id, string? note, long value, long workspace, DateOnly date, bool isPublic, bool isInvisible, DateOnly createdAt, DateOnly updatedAt, DateOnly? deletedAt, RecordStatus status)
    {
        return new Dictionary<string,object?>(){
            ["id"] = id,
            ["note"] = note,
            ["value"] = Money.Format(value),
            ["date"] = date,
            ["public"] = isPublic,
            ["invisible"] = isInvisible,
            ["createdAt"] = createdAt,
            ["updatedAt"] = updatedAt,
            ["deletedAt"] = deletedAt,
            ["status"] = RecordStatusFormatter.ToString(status),
            ["draft"] = status == RecordStatus.Draft,
            ["deleted"] = status == RecordStatus.Deleted,
            ["workspace"] = workspace,
            ["category"] = null,
            ["tags"] = new List<object>(),
            ["items"] = new List<object>(),
            ["objectives"] = new List<object>(),
            ["hidden"] = false
        };
    }

    private static Dictionary<string,object?> ViewifyHide(long id, long workspace, DateOnly date, bool isPublic, DateOnly createdAt, DateOnly updatedAt, DateOnly? deletedAt, RecordStatus status)
    {
        return new Dictionary<string,object?>(){
            ["id"] = id,
            ["note"] = "???",
            ["value"] = 0,
            ["date"] = date,
            ["public"] = isPublic,
            ["invisible"] = false,
            ["createdAt"] = createdAt,
            ["updatedAt"] = updatedAt,
            ["deletedAt"] = deletedAt,
            ["status"] = RecordStatusFormatter.ToString(status),
            ["draft"] = status == RecordStatus.Draft,
            ["deleted"] = status == RecordStatus.Deleted,
            ["workspace"] = workspace,
            ["category"] = null,
            ["tags"] = new List<object>(),
            ["items"] = new List<object>(),
            ["objectives"] = new List<object>(),
            ["hidden"] = true
        };
    }
}