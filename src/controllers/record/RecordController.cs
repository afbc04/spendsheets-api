public static class RecordController
{
    public static async Task<SendingPacket> ListRecords(QueryPage page, Dictionary<string,object?> filters, bool isHidden)
    {
        if (filters.TryGetValue("createdAt", out var createdAt))
        {
            filters.Remove("createdAt");
            filters["creation_date"] = createdAt;
        }
        if (filters.TryGetValue("updatedAt", out var updatedAt))
        {
            filters.Remove("updatedAt");
            filters["updated_date"] = updatedAt;
        }

        var daoList = await RecordRepository.List(page, filters);
        var list = daoList.List.Select(i => RecordView.ToView(i, isHidden)).ToList();
        return SendingPacket.Success(200,PageView.ToView(page, list, daoList.Count));
    }

    public static async Task<SendingPacket> GetRecord(long ID, bool isHidden)
    {
        var record = await RecordRepository.Get(ID);
        return record is not null
            ? SendingPacket.Success(200,RecordView.ToView(record, isHidden))
            : SendingPacket.Error(404, "Record does not exists");
    }

    public static async Task<SendingPacket> CreateRecord(Dictionary<string, object?> recordData)
    {
        await using var connection = await RepositoryHandler.OpenConnection();
        await using var transaction = await connection.BeginTransactionAsync();
        bool success = false;

        try
        {
            var record = new Record();

            long workspace = (long)recordData["workspace"]!;
            if (await WorkspaceRepository.Get(workspace) == null)
                return SendingPacket.Error(404, "Workspace does not exists");

            record.Value = Money.Convert64((double)recordData["value"]!);
            record.Workspace = workspace;

            if (recordData.TryGetValue("note", out var note))
                record.Note = (string?)note;

            if (recordData.TryGetValue("date", out var date))
                record.Date = (DateOnly)date!;

            if (recordData.TryGetValue("public", out var isPublic))
                record.Public = (bool)isPublic!;

            if (recordData.TryGetValue("invisible", out var invisible))
                record.Invisible = (bool)invisible!;

            if (recordData.TryGetValue("status", out var status))
            {
                var recordStatus = RecordStatusFormatter.Parse(((string)status!).ToLower());
                if (recordStatus is null)
                    return SendingPacket.Error(400, "Status of record is not valid");

                record.Status = (RecordStatus)recordStatus;
            }

            long? id;
            
            try
            {
                id = await RecordRepository.InsertWithTransaction(record, transaction);
            }
            catch
            {
                id = null;
            }

            if (id is null)
                return SendingPacket.Error(422, "Error while creating record into database");

            record.ID = (long)id;

            await transaction.CommitAsync();
            success = true;

            return SendingPacket.Success(201, RecordView.ToView(record, false));
        }
        catch (SchemaException ex)
        {
            return SendingPacket.Error(ex.statusCode, ex.message);
        }
        finally
        {
            if (!success)
                await transaction.RollbackAsync();
        }
    }

    public static async Task<SendingPacket> UpdateRecord(long ID, Dictionary<string, object?> recordData)
    {
        await using var connection = await RepositoryHandler.OpenConnection();
        await using var transaction = await connection.BeginTransactionAsync();
        bool success = false;

        try
        {
            var record = await RecordRepository.Get(ID);
            if (record is null)
                return SendingPacket.Error(404, "Record does not exists");

            if (recordData.TryGetValue("workspace", out var workspace))
            {
                long workspaceID = (long)recordData["workspace"]!;
                if (await WorkspaceRepository.Get(workspaceID) == null)
                    return SendingPacket.Error(404, "Workspace does not exists");

                record.Workspace = workspaceID;
            }

            if (recordData.TryGetValue("value", out var value))
                record.Value = Money.Convert64((double)recordData["value"]!);

            if (recordData.TryGetValue("note", out var note))
                record.Note = (string?)note;

            if (recordData.TryGetValue("date", out var date))
                record.Date = (DateOnly)date!;

            if (recordData.TryGetValue("public", out var isPublic))
                record.Public = (bool)isPublic!;

            if (recordData.TryGetValue("invisible", out var invisible))
                record.Invisible = (bool)invisible!;

            if (recordData.TryGetValue("status", out var status))
            {
                var recordStatus = RecordStatusFormatter.Parse(((string)status!).ToLower());
                if (recordStatus is null)
                    return SendingPacket.Error(400, "Status of record is not valid");

                record.Status = (RecordStatus)recordStatus;
            }

            bool wasUpdated = await RecordRepository.UpdateWithTransaction(record, transaction);
            if (!wasUpdated)
                return SendingPacket.Error(422, "Error while updating record of database");

            await transaction.CommitAsync();
            success = true;
            return SendingPacket.Success(200, RecordView.ToView(record, false));
        }
        catch (SchemaException ex)
        {
            return SendingPacket.Error(ex.statusCode, ex.message);
        }
        finally
        {
            if (!success)
                await transaction.RollbackAsync();
        }
    }

    public static async Task<SendingPacket> DeleteRecord(long id)
    {
        var record = await RecordRepository.Get(id);
        if (record is null)
            return SendingPacket.Error(404, $"Record doesn't exists");

        bool wasDeleted = await RecordRepository.Delete(id);
        if (!wasDeleted)
            return SendingPacket.Error(422, "Error while deleting record from database");

        return SendingPacket.Success(200, RecordView.ToView(record!, false));
    }
}