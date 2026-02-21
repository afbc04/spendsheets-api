using PacketHandlers;
using Controller;

public static class ControllerHelper {

    public static async Task<SendingPacket> IDIsNumber(string id, Func<long,Task<SendingPacket>> action) {

        long? id_number = Utils.to_number(id);

        if (id_number == null)
            return new PacketFail(417,"ID must be a positive integer");

        return await action((long) id_number);

    }

    public static async Task<SendingPacket> CheckIfEntryExists(EntryController entry, string entry_id, Func<Entry,Task<SendingPacket>> action) {

        long? id_number = Utils.to_number(entry_id);

        if (id_number == null)
            return new PacketFail(417,"ID must be a positive integer");

        Entry? entry_obtained = await entry._Get((long) id_number); 
        if (entry_obtained == null)
            return SendErrors.EntryDoesNotExists();

        return await action(entry_obtained);

    }

}