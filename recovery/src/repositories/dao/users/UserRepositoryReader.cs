using Npgsql;

public static class UserRepositoryReader
{
    private enum UserRepositoryEnumSimple
    {
        Username,
        Name,
        InitialMoney,
        CreationDate,
        PasswordHash,
        PasswordPassword
    }

    public static User Serialize(NpgsqlDataReader r)
    {
        return new User(
            DAOReader.getString(r, (int) UserRepositoryEnumSimple.Username),
            DAOReader.tryGetString(r, (int) UserRepositoryEnumSimple.Name),
            DAOReader.getLong(r, (int) UserRepositoryEnumSimple.InitialMoney),
            DAOReader.getDate(r, (int) UserRepositoryEnumSimple.CreationDate),
            DAOReader.getBytes(r, (int) UserRepositoryEnumSimple.PasswordHash),
            DAOReader.getBytes(r, (int) UserRepositoryEnumSimple.PasswordPassword)
        );
    }
}
