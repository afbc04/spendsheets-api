using Npgsql;

public static class ProfileRepositoryReader
{
    private enum ProfileRepositoryEnumSimple
    {
        Username,
        Name,
        CreationDate,
        IsAdmin,
        InactiveDate,
        PasswordHash,
        PasswordPassword
    }

    public static Profile Serialize(NpgsqlDataReader r)
    {
        return new Profile(
            DAOReader.getString(r, (int) ProfileRepositoryEnumSimple.Username),
            DAOReader.getString(r, (int) ProfileRepositoryEnumSimple.Name),
            DAOReader.getDate(r, (int) ProfileRepositoryEnumSimple.CreationDate),
            DAOReader.getBool(r, (int) ProfileRepositoryEnumSimple.IsAdmin),
            DAOReader.tryGetDate(r, (int) ProfileRepositoryEnumSimple.InactiveDate),
            DAOReader.getBytes(r, (int) ProfileRepositoryEnumSimple.PasswordHash),
            DAOReader.getBytes(r, (int) ProfileRepositoryEnumSimple.PasswordPassword)
        );
    }
}
