public class EntryNote {

    public long ID {set; get;}
    public string note {set; get;}
    public DateOnly changeDate {set; get;}

    public EntryNote(long ID, string note, DateOnly date) {

        this.ID = ID;
        this.note = note;
        this.changeDate = date;

    }

    public EntryNote(long ID, string note) {

        this.ID = ID;
        this.note = note;
        this.changeDate = DateOnly.FromDateTime(DateTime.UtcNow);

    }

    public EntryNote(EntryNote en) {

        this.ID = en.ID;
        this.note = en.note;
        this.changeDate = en.changeDate;

    }

    public IDictionary<string,object?> ToJson() {
        return new Dictionary<string,object?> {
            ["id"] = this.ID,
            ["note"] = this.note,
            ["changeDate"] = this.changeDate
        };
    }

}