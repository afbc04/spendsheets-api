public static class EntryTypeHandler {

    public static readonly IReadOnlyList<string> types = new List<string> { "transaction", "commitment", "savings" };


    public static string Get(EntryType type) {

        return type switch {
            EntryType.Transaction => "transaction",
            EntryType.Commitment => "commitment",
            EntryType.Saving => "savings",
            _ => "???"
        };

    }

    public static EntryType? Get(string type) {

        return type.ToLower() switch {
            "transaction" => EntryType.Transaction,
            "commitment" => EntryType.Commitment,
            "savings" => EntryType.Saving,
            _ => null
        };

    }

    public static EntryType exportDAO(char type) {
        return (EntryType) (type - '0');
    }

    public static char importDAO(EntryType type) {
        return (char) ('0' + ((int) type));
    }

}