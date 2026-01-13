using PacketHandlers;
using Controller;

public static class ControllerHelper {

    public static async Task<SendingPacket> IDIsNumber(string id, Func<long,Task<SendingPacket>> action) {

        long? id_number = Utils.to_number(id);

        if (id_number == null)
            return new PacketFail(417,"ID must be a positive integer");

        return await action((long) id_number);

    }

    public static async Task<SendingPacket> CheckIfEntryExists(EntryController entry, string entry_id, Func<long,Task<SendingPacket>> action) {

        long? id_number = Utils.to_number(entry_id);

        if (id_number == null)
            return new PacketFail(417,"ID must be a positive integer");

        if (await entry._Contains((long) id_number) == false)
            return SendErrors.EntryDoesNotExists();

        return await action((long) id_number);

    }

}