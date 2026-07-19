public static class WorkspaceController
{
    public static async Task<SendingPacket> ListWorkspace(bool isHidden)
    {
        var workspaces = await WorkspaceRepository.List();
        var list = workspaces.Select(w => WorkspaceView.ToView(w, isHidden));
        return SendingPacket.Success(200, list);
    }

    public static async Task<SendingPacket> GetWorkspace(long id, bool isHidden)
    {
        var workspace = await WorkspaceRepository.Get(id);
        return workspace is not null
            ? SendingPacket.Success(200,WorkspaceView.ToView(workspace, isHidden))
            : SendingPacket.Error(404, "Workspace does not exists");
    }
    
    public static async Task<SendingPacket> CreateWorkspace(Dictionary<string, object?> workspaceData)
    {
        try
        {
            var workspace = new Workspace();

            workspace.Name = (string)workspaceData["name"]!;

            if (workspaceData.TryGetValue("description", out var description))
                workspace.Description = (string?)description!;

            if (workspaceData.TryGetValue("initialMoney", out var initialMoney))
                workspace.InitialMoney = Money.Convert64((double)initialMoney!);

            long? id = await WorkspaceRepository.Insert(workspace);

            if (id is null)
                return SendingPacket.Error(422, "Error while creating workspace into database");

            workspace.ID = (long)id;
            return SendingPacket.Success(201, WorkspaceView.ToView(workspace, false));
        }
        catch (SchemaException ex)
        {
            return SendingPacket.Error(ex.statusCode, ex.message);
        }
    }

    public static async Task<SendingPacket> UpdateWorkspace(long id, Dictionary<string, object?> workspaceData)
    {
        try
        {
            var workspace = await WorkspaceRepository.Get(id);
            if (workspace is null)
                return SendingPacket.Error(404, $"Workspace doesn't exists");

            if (workspaceData.TryGetValue("name", out var name))
                workspace.Name = (string)name!;

            if (workspaceData.TryGetValue("description", out var description))
                workspace.Description = (string?)description!;

            if (workspaceData.TryGetValue("initialMoney", out var initialMoney))
                workspace.InitialMoney = Money.Convert64((double)initialMoney!);

            bool wasUpdated = await WorkspaceRepository.Update(workspace);
            if (!wasUpdated)
                return SendingPacket.Error(422, "Error while updating workspace of database");

            return SendingPacket.Success(200, WorkspaceView.ToView(workspace!, false));
        }
        catch (SchemaException ex)
        {
            return SendingPacket.Error(ex.statusCode, ex.message);
        }
    }

    public static async Task<SendingPacket> DeleteWorkspace(long id)
    {
        var workspace = await WorkspaceRepository.Get(id);
        if (workspace is null)
            return SendingPacket.Error(404, $"Workspace doesn't exists");

        bool wasDeleted = await WorkspaceRepository.Delete(id);
        if (!wasDeleted)
            return SendingPacket.Error(422, "Error while deleting workspace from database");

        return SendingPacket.Success(200, WorkspaceView.ToView(workspace!, false));
    }
}