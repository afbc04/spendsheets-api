using Npgsql;

public static class TagsRepositoryReader
{
    private enum TagsRepositoryEnum
    {
        ID,
        Name,
        Description
    }

    public static TagModel Serialize(NpgsqlDataReader r)
    {
        return new TagModel(
            DAOReader.getLong(r, (int) TagsRepositoryEnum.ID),
            DAOReader.getString(r, (int) TagsRepositoryEnum.Name),
            DAOReader.tryGetString(r, (int) TagsRepositoryEnum.Description)
        );
    }

    private enum TagsRepositoryEnumList
    {
        ID,
        Name
    }

    public static TagModelList SerializeList(NpgsqlDataReader r)
    {
        return new TagModelList(
            DAOReader.getLong(r, (int) TagsRepositoryEnumList.ID),
            DAOReader.getString(r, (int) TagsRepositoryEnumList.Name)
        );
    }
}
