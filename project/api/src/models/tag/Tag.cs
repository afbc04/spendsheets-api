public class Tag {

    public long ID {set; get;}
    public string name {set; get;}
    public string? description {set; get;}

    public Tag(long ID, string name) {

        this.ID = ID;
        this.name = name;
        this.description = null;

    }

    public Tag(long ID, string name, string? description) {

        this.ID = ID;
        this.name = name;
        this.description = description;

    }

    public IDictionary<string,object?> to_json() {
        return new Dictionary<string,object?> {
            ["id"] = this.ID,
            ["name"] = this.name,
            ["description"] = this.description
        };
    }


}