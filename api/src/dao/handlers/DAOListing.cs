namespace DAO {

    public class DAOListing<T> {

        public long count { get; }
        public List<T> list { get; }

        public DAOListing(long count, List<T> list){
            this.count = count;
            this.list = list;
        }

    }

}