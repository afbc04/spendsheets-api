public static class WorkspaceView
{
    public static Dictionary<string,object?> ToView(Workspace workspace, bool hidden)
        => hidden
            ? ViewifyHide(workspace.ID, workspace.Name, workspace.Description)
            : ViewifyShow(workspace.ID, workspace.Name, workspace.Description, workspace.InitialMoney);

    private static Dictionary<string,object?> ViewifyShow(long id, string name, string? description, long initialMoney)
    {
        return new Dictionary<string,object?>(){
            ["id"] = id,
            ["name"] = name,
            ["description"] = description,
            ["initialMoney"] = Money.Format(initialMoney),
            ["hidden"] = false
        };
    }

    private static Dictionary<string,object?> ViewifyHide(long id, string name, string? description)
    {
        return new Dictionary<string,object?>(){
            ["id"] = id,
            ["name"] = name,
            ["description"] = description,
            ["initialMoney"] = null,
            ["hidden"] = true
        };
    }
}