public class EntryException : Exception {

    public string message {set; get;}

    public EntryException(string message) {
        this.message = message;
    }

}