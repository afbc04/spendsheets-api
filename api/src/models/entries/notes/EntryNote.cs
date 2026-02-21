public class EntryNote {

    public long ID {set; get;}
    public DateOnly date {set; get;}
    public string? note {set; get;}
    public int? money {set; get;}

    public EntryNote(long ID, DateOnly date, int? money, string? note) {

        this.ID = ID;
        this.note = note;
        this.money = money;
        this.date = date;

    }

    public EntryNote(long ID) {

        this.ID = ID;
        this.note = null;
        this.money = null;
        this.date = DateOnly.FromDateTime(DateTime.UtcNow);

    }

    public EntryNote(EntryNote en) {

        this.ID = en.ID;
        this.note = en.note;
        this.money = en.money;
        this.date = en.date;


    }

    /*
    public IDictionary<string,object?> ToJson() {
        return new Dictionary<string,object?> {
            ["id"] = this.ID,
            ["money"] = Money.Format((int) this.money!),
            ["date"] = this.date,
            ["note"] = this.note
        };
    }*/

}
