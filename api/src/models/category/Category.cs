public class Category {

    public long ID {set; get;}
    public string name {set; get;}
    public string? description {set; get;}

    public Category(long ID, string name) {

        this.ID = ID;
        this.name = name;
        this.description = null;

    }

    public Category(long ID, string name, string? description) {

        this.ID = ID;
        this.name = name;
        this.description = description;

    }

}