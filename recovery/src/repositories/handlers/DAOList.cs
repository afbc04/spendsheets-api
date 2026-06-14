public class DAOList<T>(long count, List<T> list)
{
    public long Count { get; } = count;
    public List<T> List { get; } = list;
}