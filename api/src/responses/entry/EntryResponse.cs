public static class EntryResponse {

    public static IDictionary<string,object?> ToJson(EntryDetails entry) => ToJson(entry.entry,entry.category,entry.collection,true);
    public static IDictionary<string,object?> ToJson(EntryDetails entry, bool can_read) => ToJson(entry.entry,entry.category,entry.collection,can_read);
    public static IDictionary<string,object?> ToJson(Entry entry, Category? category, Collection? collection) => _show(entry,category,collection);

    public static IDictionary<string,object?> ToJson(Entry entry, Category? category, Collection? collection, bool can_read) =>
        can_read ? _show(entry,category,collection) : _hide(entry,category,collection);




    private static IDictionary<string,object?> _show(Entry entry, Category? category, Collection? collection) =>
        new Dictionary<string,object?> {
            ["id"] = entry.ID,
            ["category"] = _show_category(category),
            ["collection"] = _show_collection(collection),
            ["visible"] = entry.is_visible,
            ["public"] = entry.is_public,
            ["active"] = entry.active,
            ["type"] = EntryTypeHandler.Get(entry.type),
            ["date"] = entry.date,
            ["description"] = entry.description,
            ["targetMoney"] = _get_target_money(entry),
            ["actualMoney"] = _get_actual_money(entry),
            ["remainingMoney"] = _get_remaining_money(entry),
            ["lastChangeDate"] = Utils.ConvertToDateTime(entry.last_change_date),
            ["creationDate"] = entry.creation_date,
            ["finishDate"] = entry.finish_date,
            ["dueDate"] = entry.due_date,
            ["draft"] = entry.status == EntryStatus.Draft,
            ["finished"] = entry.status == EntryStatus.Done,
            ["completed"] = entry.status == EntryStatus.Completed,
            ["pending"] = entry.status == EntryStatus.Pending,
            ["stalled"] = entry.status == EntryStatus.Stalled,
            ["deleted"] = EntryStatusHandler.IsDeleted(entry.status),
            ["deletedDate"] = entry.deleted_date,
            ["status"] = EntryStatusHandler.Get(entry),
            ["_links"] = _get_links(entry),
            ["hidden"] = false
        }; 

    private static IDictionary<string,object?> _hide(Entry entry, Category? category, Collection? collection) =>
        new Dictionary<string,object?> {
            ["id"] = entry.ID,
            /*["name"] = "???",
            ["description"] = null,
            ["isMonthlyService"] = collection.is_monthly_service,
            ["monthlyService"] = collection.is_monthly_service ? _hide_monthly_service(collection,category) : null,*/
            ["hidden"] = true
        }; 


    private static Dictionary<string, object?> _get_links(Entry entry) {

        string url = $"/v1.0/entries/{entry.ID}";

        return new Dictionary<string, object?> {
            ["self"] = url,
            ["notes"] = $"{url}/notes"
        };

    }

    private static decimal? _get_actual_money(Entry entry) {
        return entry.money_spent == null ? Money.Format(entry.money) : Money.Format((int) entry.money_spent);
    }

    private static decimal? _get_target_money(Entry entry) {
        return entry.money_spent != null ? Money.Format(entry.money) : null;
    }

    private static decimal? _get_remaining_money(Entry entry) {
        return entry.money_spent != null ? Money.Format((long) (entry.money - entry.money_spent)) : null;
    }

    private static IDictionary<string,object?>? _show_category(Category? category) =>
        category != null ? new Dictionary<string,object?> {
            ["id"] = category.ID,
            ["name"] = category.name,
            ["description"] = category.description,
            ["hidden"] = false
        } : null; 

    private static IDictionary<string,object?>? _hide_category(Category? category) =>
        category != null ? new Dictionary<string,object?> {
            ["id"] = category.ID,
            ["name"] = "???",
            ["description"] = null,
            ["hidden"] = true
        } : null; 



    private static IDictionary<string,object?>? _show_collection(Collection? collection) =>
        collection != null ? new Dictionary<string,object?> {
            ["id"] = collection.ID,
            ["name"] = collection.name,
            ["description"] = collection.description,
            ["monthlyService"] = collection.is_monthly_service,
            ["hidden"] = false
        } : null; 

    private static IDictionary<string,object?>? _hide_collection(Collection? collection) =>
        collection != null ? new Dictionary<string,object?> {
            ["id"] = collection.ID,
            ["name"] = "???",
            ["description"] = null,
            ["monthlyService"] = collection.is_monthly_service,
            ["hidden"] = true
        } : null; 


}