using Npgsql;

public static class RecordsRepositoryReader
{

    private enum RecordsRepositoryEnum
    {
        ID,
        Note,
        Value,
        Date,
        Workspace,
        Invisible,
        Public,
        CreationDate,
        UpdatedDate,
        DeletionDate,
        Status
    }

    public static Record Serialize(NpgsqlDataReader r)
    {
        return new(
            DAOReader.getLong(r, (int) RecordsRepositoryEnum.ID),
            DAOReader.tryGetString(r, (int) RecordsRepositoryEnum.Note),
            DAOReader.getLong(r, (int) RecordsRepositoryEnum.Value),
            DAOReader.getDate(r, (int) RecordsRepositoryEnum.Date),
            DAOReader.getLong(r, (int) RecordsRepositoryEnum.Workspace),
            DAOReader.getBool(r, (int) RecordsRepositoryEnum.Invisible),
            DAOReader.getBool(r, (int) RecordsRepositoryEnum.Public),
            DAOReader.getDate(r, (int) RecordsRepositoryEnum.CreationDate),
            DAOReader.getDate(r, (int) RecordsRepositoryEnum.UpdatedDate),
            DAOReader.tryGetDate(r, (int) RecordsRepositoryEnum.DeletionDate),
            RecordStatusFormatter.Export(DAOReader.getInt(r, (int) RecordsRepositoryEnum.Status))
        );
    }
}
