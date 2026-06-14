using Npgsql;

public static class CategoriesRepositoryReader
{

    private enum CategoryRepositoryEnum
    {
        ID,
        Name,
        Description,
        ParentID,
        ParentName,
        CreationDate
    }

    public static CategoryModel Serialize(NpgsqlDataReader r)
    {
        return new(
            DAOReader.getLong(r, (int) CategoryRepositoryEnum.ID),
            DAOReader.getString(r, (int) CategoryRepositoryEnum.Name),
            DAOReader.tryGetString(r, (int) CategoryRepositoryEnum.Description),
            DAOReader.tryGetLong(r, (int) CategoryRepositoryEnum.ParentID),
            DAOReader.tryGetString(r, (int) CategoryRepositoryEnum.ParentName),
            DAOReader.getDate(r, (int) CategoryRepositoryEnum.CreationDate)
        );
    }

    private enum CategoryRepositoryWithSubcategoryCountsEnum
    {
        ID,
        Name,
        Description,
        ParentID,
        ParentName,
        CreationDate,
        ChildsCount
    }

    public static CategoryModelWithSubcategoryCount SerializeWithSubcategoryCount(NpgsqlDataReader r)
    {
        return new(
            DAOReader.getLong(r, (int) CategoryRepositoryWithSubcategoryCountsEnum.ID),
            DAOReader.getString(r, (int) CategoryRepositoryWithSubcategoryCountsEnum.Name),
            DAOReader.tryGetString(r, (int) CategoryRepositoryWithSubcategoryCountsEnum.Description),
            DAOReader.tryGetLong(r, (int) CategoryRepositoryWithSubcategoryCountsEnum.ParentID),
            DAOReader.tryGetString(r, (int) CategoryRepositoryWithSubcategoryCountsEnum.ParentName),
            DAOReader.getDate(r, (int) CategoryRepositoryWithSubcategoryCountsEnum.CreationDate),
            DAOReader.getLong(r, (int) CategoryRepositoryWithSubcategoryCountsEnum.ChildsCount)
        );
    }

    private enum CategoryRepositoryEnumParentDetails
    {
        ID,
        Name,
        HasParent
    }

    public static CategoryModelParent SerializeParent(NpgsqlDataReader r)
    {
        return new CategoryModelParent(
            DAOReader.getLong(r, (int) CategoryRepositoryEnumParentDetails.ID),
            DAOReader.getString(r, (int) CategoryRepositoryEnumParentDetails.Name),
            DAOReader.getBool(r, (int) CategoryRepositoryEnumParentDetails.HasParent)
        );
    }

    private enum CategoryRepositoryEnumList
    {
        ID,
        Name,
        Description,
        ParentID,
        ParentName,
        CreationDate
    }

    public static CategoryModelList SerializeList(NpgsqlDataReader r)
    {
        return new CategoryModelList(
            DAOReader.getLong(r, (int) CategoryRepositoryEnumList.ID),
            DAOReader.getString(r, (int) CategoryRepositoryEnumList.Name),
            DAOReader.tryGetString(r, (int) CategoryRepositoryEnumList.Description),
            DAOReader.tryGetLong(r, (int) CategoryRepositoryEnumList.ParentID),
            DAOReader.tryGetString(r, (int) CategoryRepositoryEnumList.ParentName),
            DAOReader.getDate(r, (int) CategoryRepositoryEnumList.CreationDate)
        );
    }
}
