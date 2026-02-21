public static class CollectionListResponse {

    public static IDictionary<string,object?> ToJson(CollectionList collection) => _show(collection);

    public static IDictionary<string,object?> ToJson(CollectionList collection, bool can_read) =>
        can_read ? _show(collection) : _hide(collection);




    private static IDictionary<string,object?> _show(CollectionList collection) =>
        new Dictionary<string,object?> {
            ["id"] = collection.ID,
            ["name"] = collection.name,
            ["isMonthlyService"] = collection.is_monthly_service,
            ["monthlyServiceActive"] = collection.is_monthly_service_active,
            ["hidden"] = false
        }; 

    private static IDictionary<string,object?> _hide(CollectionList collection) =>
        new Dictionary<string,object?> {
            ["id"] = collection.ID,
            ["name"] = "???",
            ["isMonthlyService"] = collection.is_monthly_service,
            ["monthlyServiceActive"] = collection.is_monthly_service_active,
            ["hidden"] = true
        }; 

}