public static class EntryTypeHandler {

    public static readonly IReadOnlyList<string> types = new List<string> { "payment", "loan" };


    public static string Get(EntryType type) {

        return type switch {
            EntryType.Payment => "payment",
            EntryType.Loan => "loan",
            _ => "???"
        };

    }

    public static EntryType? Get(string type) {

        return type.ToLower() switch {
            "payment" => EntryType.Payment,
            "loan" => EntryType.Loan,
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