using PacketHandlers;

public static class ControllerHelper {

    public static async Task<SendingPacket> IDIsNumber(string id, Func<long,Task<SendingPacket>> action) {

        long? id_number = Utils.to_number(id);

        if (id_number == null)
            return new PacketFail(417,"ID must be a positive integer");

        return await action((long) id_number);

    }

}