namespace Queries;
using Pages;

public interface IQuery {
    public string get_sql_listing();
    public string get_sql_filtering();
    public IList<string> validate();
    public PageInput get_page();
}