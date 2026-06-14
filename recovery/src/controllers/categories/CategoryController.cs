public static class CategoryController
{/*
    public static async Task<SendingPacket> ListCategories(QueryPage page, Dictionary<string,object?> filters, bool isHidden)
    {
        var daoList = await CategoryRepository.List(page, filters);
        var list = daoList.List.Select(i => CategoryListView.ToView(i, isHidden)).ToList();
        return SendingPacket.Success(200,PageView.ToView(page, list, daoList.Count)); 
    }

    public static async Task<SendingPacket> CreateCategory(Dictionary<string, object?> categoryData)
    {
        await using var connection = await RepositoryHandler.OpenConnection();
        await using var transaction = await connection.BeginTransactionAsync();
        bool success = false;

        try
        {
            var category = new Category();
            CategoryModelParent? categoryParent = null;

            category.Name = (string)categoryData["name"]!;

            if (categoryData.TryGetValue("description", out var description))
                category.Description = (string?)description;

            if (categoryData.TryGetValue("parentId", out var parentId))
            {
                if (parentId is not null)
                {
                    categoryParent = await CategoryRepository.GetParentLockReader((long)parentId, transaction);
    
                    if (categoryParent is null)
                        return SendingPacket.Error(ErrorCategory.CATEGORY_PARENT_NOT_EXISTS);

                    if (categoryParent.HasParent)
                        return SendingPacket.Error(ErrorCategory.CATEGORY_PARENT_IS_SUBCATEGORY);
                }

                category.ParentID = (long?)parentId;
            }

            long? id = await CategoryRepository.InsertWithTransaction(category, transaction);

            if (id is null)
                return SendingPacket.Error(ErrorCategory.CATEGORY_FAIL_CREATING);

            category.ID = id.Value;

            await transaction.CommitAsync();
            success = true;

            return SendingPacket.Success(201, CategoryView.ToView(category, categoryParent));
        }
        catch (SchemaException ex)
        {
            return SendingPacket.Error(ex.ErrorId);
        }
        finally
        {
            if (!success)
                await transaction.RollbackAsync();
        }
    }

    public static async Task<SendingPacket> GetCategory(long ID, bool isHidden)
    {
        var category = await CategoryRepository.Get(ID);
        return category is not null
            ? SendingPacket.Success(200,CategoryView.ToView(category, isHidden))
            : SendingPacket.Error(ErrorCategory.CATEGORY_NOT_EXISTS);
    }

    public static async Task<SendingPacket> DeleteCategory(long ID)
    {
        var category = await CategoryRepository.Get(ID);

        if (category is null)
            return SendingPacket.Error(ErrorCategory.CATEGORY_NOT_EXISTS);
        
        return await CategoryRepository.Delete(ID)
            ? SendingPacket.Success(200,CategoryView.ToView(category, false))
            : SendingPacket.Error(ErrorCategory.CATEGORY_FAIL_DELETING);
    }

    public static async Task<SendingPacket> UpdateCategory(long ID, Dictionary<string, object?> categoryData)
    {
        await using var connection = await RepositoryHandler.OpenConnection();
        await using var transaction = await connection.BeginTransactionAsync();
        bool success = false;

        try
        {
            var category = await CategoryRepository.GetWithSubcategoryCountLockWriter(ID, transaction);
            if (category is null)
                return SendingPacket.Error(ErrorCategory.CATEGORY_NOT_EXISTS);

            var updatedCategory = new Category(ID, category.CreationDate);
            CategoryModelParent? categoryParent = null;

            updatedCategory.Name = (string)categoryData["name"]!;

            if (categoryData.TryGetValue("description", out var description))
                updatedCategory.Description = (string?)description;

            if (categoryData.TryGetValue("parentId", out var parentId))
            {
                if (parentId is not null) 
                {

                    if (ID == (long) parentId!)
                        return SendingPacket.Error(ErrorCategory.CATEGORY_SELF_PARENT);

                    categoryParent = await CategoryRepository.GetParentLockReader((long)parentId, transaction);
                    
                    if (categoryParent is null)
                        return SendingPacket.Error(ErrorCategory.CATEGORY_PARENT_NOT_EXISTS);

                    if (categoryParent.HasParent)
                        return SendingPacket.Error(ErrorCategory.CATEGORY_PARENT_IS_SUBCATEGORY);

                    if (category.ChildsCount > 0)
                        return SendingPacket.Error(ErrorCategory.CATEGORY_PARENT_CAN_NOT_BE_SUBCATEGORY);
                }

                updatedCategory.ParentID = (long?) parentId;
            }

            if (await CategoryRepository.UpdateWithTransaction(updatedCategory, transaction)) 
            {
                await transaction.CommitAsync();
                success = true;
                return SendingPacket.Success(200,CategoryView.ToView(updatedCategory, categoryParent));
            }
            
            return SendingPacket.Error(ErrorCategory.CATEGORY_FAIL_UPDATING);
        }
        catch (SchemaException ex)
        {
            return SendingPacket.Error(ex.ErrorId);
        }
        finally
        {
            if (!success)
                await transaction.RollbackAsync();
        }
    }

    public static async Task<SendingPacket> PatchCategory(long ID, Dictionary<string, object?> categoryData)
    {
        await using var connection = await RepositoryHandler.OpenConnection();
        await using var transaction = await connection.BeginTransactionAsync();
        bool success = false;

        try
        {
            var category = await CategoryRepository.GetWithSubcategoryCountLockWriter(ID, transaction);
            if (category is null)
                return SendingPacket.Error(ErrorCategory.CATEGORY_NOT_EXISTS);

            var updatedCategory = new Category(category);
            CategoryModelParent? categoryParent = null;

            if (categoryData.TryGetValue("name", out var name))
                updatedCategory.Name = (string)name!;

            if (categoryData.TryGetValue("description", out var description))
                updatedCategory.Description = (string?)description;

            if (categoryData.TryGetValue("parentId", out var parentId))
            {
                if (parentId is not null) 
                {

                    if (ID == (long) parentId!)
                        return SendingPacket.Error(ErrorCategory.CATEGORY_SELF_PARENT);

                    categoryParent = await CategoryRepository.GetParentLockReader((long)parentId, transaction);
                    
                    if (categoryParent is null)
                        return SendingPacket.Error(ErrorCategory.CATEGORY_PARENT_NOT_EXISTS);

                    if (categoryParent.HasParent)
                        return SendingPacket.Error(ErrorCategory.CATEGORY_PARENT_IS_SUBCATEGORY);

                    if (category.ChildsCount > 0)
                        return SendingPacket.Error(ErrorCategory.CATEGORY_PARENT_CAN_NOT_BE_SUBCATEGORY);
                }

                updatedCategory.ParentID = (long?) parentId;
            }
            else
                categoryParent = category.ParentID is not null ? new CategoryModelParent((long)category.ParentID,(string)category.ParentName!, false) : null;

            if (await CategoryRepository.UpdateWithTransaction(updatedCategory, transaction)) 
            {
                await transaction.CommitAsync();
                success = true;
                return SendingPacket.Success(200,CategoryView.ToView(updatedCategory, categoryParent));
            }
            
            return SendingPacket.Error(ErrorCategory.CATEGORY_FAIL_UPDATING);
        }
        catch (SchemaException ex)
        {
            return SendingPacket.Error(ex.ErrorId);
        }
        finally
        {
            if (!success)
                await transaction.RollbackAsync();
        }
    }
    */
}