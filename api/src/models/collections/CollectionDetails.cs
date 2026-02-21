public class CollectionDetails {

    public Collection collection {set; get;}
    public Category? category {set; get;}

    public CollectionDetails(Collection collection, Category? category) {

        this.collection = collection;
        this.category = category;

    }

}