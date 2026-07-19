public static class WorkspaceView
{
    public static Dictionary<string,object?> ToView(Workspace workspace, bool hidden)
        => ViewifyShow(workspace.ID, workspace.Name, workspace.Description, workspace.InitialMoney);

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
}