public static class EntryNoteResponse {

    public static IDictionary<string,object?> ToJson(EntryNote note) => ToJson(note,true);
    public static IDictionary<string,object?> ToJson(EntryNote note, bool can_read) =>
        can_read ? _show(note) : _hide(note);




    private static IDictionary<string,object?> _show(EntryNote note) =>
        new Dictionary<string,object?> {
            ["id"] = note.ID,
            ["date"] = note.date,
            ["money"] = note.money != null ? Money.Format((int) note.money) : null,
            ["note"] = note.note,
            ["hidden"] = false
        }; 

    private static IDictionary<string,object?> _hide(EntryNote note) =>
        new Dictionary<string,object?> {
            ["id"] = note.ID,
            ["date"] = note.date,
            ["money"] = null,
            ["note"] = null,
            ["hidden"] = true
        }; 

}