using Npgsql;

public static class WorkspaceRepositoryReader
{

    private enum WorkspaceRepositoryEnum
    {
        ID,
        Name,
        Description,
        InitialMoney
    }

    public static Workspace Serialize(NpgsqlDataReader r)
    {
        return new(
            DAOReader.getLong(r, (int) WorkspaceRepositoryEnum.ID),
            DAOReader.getString(r, (int) WorkspaceRepositoryEnum.Name),
            DAOReader.tryGetString(r, (int) WorkspaceRepositoryEnum.Description),
            DAOReader.getLong(r, (int) WorkspaceRepositoryEnum.InitialMoney)
        );
    }
}
