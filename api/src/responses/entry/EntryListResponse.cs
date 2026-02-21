public static class EntryListResponse {

    public static IDictionary<string,object?> ToJson(EntryList entry) => ToJson(entry,true);
    public static IDictionary<string,object?> ToJson(EntryList entry, bool can_read) =>
        can_read ? _show(entry) : _hide(entry);




    private static IDictionary<string,object?> _show(EntryList entry) =>
        new Dictionary<string,object?> {
            ["id"] = entry.ID,
            ["category"] = _show_category(entry),
            ["collection"] = _show_collection(entry),
            ["visible"] = entry.is_visible,
            ["public"] = entry.is_public,
            ["active"] = entry.is_active,
            ["type"] = EntryTypeHandler.Get(entry.type),
            ["date"] = entry.date,
            ["targetMoney"] = _get_target_money(entry),
            ["actualMoney"] = _get_actual_money(entry),
            ["remainingMoney"] = _get_remaining_money(entry),
            ["lastChangeDate"] = Utils.ConvertToDateTime(entry.last_change_date),
            ["dueDate"] = entry.due_date,
            ["status"] = EntryStatusHandler.Get(entry),
            ["hidden"] = false
        }; 

    private static IDictionary<string,object?> _hide(EntryList entry) =>
        new Dictionary<string,object?> {
            ["id"] = entry.ID,
            /*["name"] = "???",
            ["description"] = null,
            ["isMonthlyService"] = collection.is_monthly_service,
            ["monthlyService"] = collection.is_monthly_service ? _hide_monthly_service(collection,category) : null,*/
            ["hidden"] = true
        }; 

    private static decimal? _get_actual_money(EntryList entry) {
        return entry.money_spent == null ? Money.Format(entry.money) : Money.Format((int) entry.money_spent);
    }

    private static decimal? _get_target_money(EntryList entry) {
        return entry.money_spent != null ? Money.Format(entry.money) : null;
    }

    private static decimal? _get_remaining_money(EntryList entry) {
        return entry.money_spent != null ? Money.Format((long) (entry.money - entry.money_spent)) : null;
    }

    private static IDictionary<string,object?>? _show_category(EntryList entry) =>
        entry.category_ID != null ? new Dictionary<string,object?> {
            ["id"] = entry.category_ID,
            ["name"] = entry.category_name,
            ["hidden"] = false
        } : null; 

    private static IDictionary<string,object?>? _hide_category(EntryList entry) =>
        entry.category_ID != null ? new Dictionary<string,object?> {
            ["id"] = entry.category_ID,
            ["name"] = "???",
            ["hidden"] = true
        } : null; 



    private static IDictionary<string,object?>? _show_collection(EntryList entry) =>
        entry.collection_ID != null ? new Dictionary<string,object?> {
            ["id"] = entry.collection_ID,
            ["name"] = entry.collection_name,
            ["monthlyService"] = entry.collection_is_monthly_service,
            ["hidden"] = false
        } : null; 

    private static IDictionary<string,object?>? _hide_collection(EntryList entry) =>
        entry.collection_ID != null ? new Dictionary<string,object?> {
            ["id"] = entry.collection_ID,
            ["name"] = "???",
            ["monthlyService"] = entry.collection_is_monthly_service,
            ["hidden"] = true
        } : null; 


}