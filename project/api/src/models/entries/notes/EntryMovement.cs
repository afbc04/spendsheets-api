public class EntryMovement {

    public long ID {set; get;}
    public string? comment {set; get;}
    public int money {set; get;}
    public DateOnly date {set; get;}

    public EntryMovement(long ID, int money, DateOnly date, string? comment) {

        this.ID = ID;
        this.comment = comment;
        this.money = money;
        this.date = date;

    }

    public EntryMovement(long ID, int money) {

        this.ID = ID;
        this.comment = null;
        this.money = money;
        this.date = DateOnly.FromDateTime(DateTime.UtcNow);

    }

    public EntryMovement(EntryMovement em) {

        this.ID = em.ID;
        this.comment = em.comment;
        this.money = em.money;
        this.date = em.date;


    }

    public IDictionary<string,object?> ToJson() {
        return new Dictionary<string,object?> {
            ["id"] = this.ID,
            ["money"] = Money.Format(this.money),
            ["date"] = this.date,
            ["comment"] = this.comment
        };
    }

}