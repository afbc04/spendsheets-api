public static class TagsController
{/*
    public static async Task<SendingPacket> ListTags(QueryPage page, Dictionary<string,object?> filters, bool isHidden)
    {
        var daoList = await TagsRepository.List(page, filters);
        var list = daoList.List.Select(i => TagListView.ToView(i, isHidden)).ToList();
        return SendingPacket.Success(200,PageView.ToView(page, list, daoList.Count)); 
    }

    public static async Task<SendingPacket> CreateTag(Dictionary<string, object?> tagData)
    {
        try
        {
            var tag = new Tag();

            tag.Name = (string)tagData["name"]!;

            if (tagData.TryGetValue("description", out var description))
                tag.Description = (string?)description;

            long? id = await TagsRepository.Insert(tag);

            if (id is null)
                return SendingPacket.Error(ErrorCategory.TAG_FAIL_CREATING);

            tag.ID = id.Value;
            return SendingPacket.Success(201, TagView.ToView(tag));
        }
        catch (SchemaException ex)
        {
            return SendingPacket.Error(ex.ErrorId);
        }
    }

    public static async Task<SendingPacket> GetTag(long ID, bool isHidden)
    {
        var tag = await TagsRepository.Get(ID);
        return tag is not null
            ? SendingPacket.Success(200,TagView.ToView(tag, isHidden))
            : SendingPacket.Error(ErrorCategory.TAG_NOT_EXISTS);
    }

    public static async Task<SendingPacket> DeleteTag(long ID)
    {
        var tag = await TagsRepository.Get(ID);

        if (tag is null)
            return SendingPacket.Error(ErrorCategory.TAG_NOT_EXISTS);
        
        return await TagsRepository.Delete(ID)
            ? SendingPacket.Success(200,TagView.ToView(tag, false))
            : SendingPacket.Error(ErrorCategory.TAG_FAIL_DELETING);
    }

    public static async Task<SendingPacket> UpdateTag(long ID, Dictionary<string, object?> tagData)
    {
        try
        {
            var tagObtainedFromDatabase = await TagsRepository.Get(ID);

            if (tagObtainedFromDatabase is null)
                return SendingPacket.Error(ErrorCategory.TAG_NOT_EXISTS);

            var tag = new Tag(ID);

            tag.Name = (string)tagData["name"]!;

            if (tagData.TryGetValue("description", out var description))
                tag.Description = (string?)description;

            return await TagsRepository.Update(tag)
                ? SendingPacket.Success(200,TagView.ToView(tag))
                : SendingPacket.Error(ErrorCategory.TAG_FAIL_UPDATING);
        }
        catch (SchemaException ex)
        {
            return SendingPacket.Error(ex.ErrorId);
        }
    }

    public static async Task<SendingPacket> PatchTag(long ID, Dictionary<string, object?> tagData)
    {
        try
        {
            var tagObtainedFromDatabase = await TagsRepository.Get(ID);

            if (tagObtainedFromDatabase is null)
                return SendingPacket.Error(ErrorCategory.TAG_NOT_EXISTS);

            var tag = new Tag(ID, tagObtainedFromDatabase.Name, tagObtainedFromDatabase.Description);

            if (tagData.TryGetValue("name", out var name))
                tag.Name = (string)name!;

            if (tagData.TryGetValue("description", out var description))
                tag.Description = (string?)description;

            return await TagsRepository.Update(tag)
                ? SendingPacket.Success(200,TagView.ToView(tag))
                : SendingPacket.Error(ErrorCategory.TAG_FAIL_UPDATING);
        }
        catch (SchemaException ex)
        {
            return SendingPacket.Error(ex.ErrorId);
        }
    }*/
}