public static class CollectionResponse {

    public static IDictionary<string,object?> ToJson(CollectionDetails collection) => ToJson(collection.collection,collection.category,true);
    public static IDictionary<string,object?> ToJson(CollectionDetails collection, bool can_read) => ToJson(collection.collection,collection.category,can_read);
    public static IDictionary<string,object?> ToJson(Collection collection, Category? category) => _show(collection,category);

    public static IDictionary<string,object?> ToJson(Collection collection, Category? category, bool can_read) =>
        can_read ? _show(collection,category) : _hide(collection,category);




    private static IDictionary<string,object?> _show(Collection collection, Category? category) =>
        new Dictionary<string,object?> {
            ["id"] = collection.ID,
            ["name"] = collection.name,
            ["description"] = collection.description,
            ["isMonthlyService"] = collection.is_monthly_service,
            ["monthlyService"] = collection.is_monthly_service ? _show_monthly_service(collection,category) : null,
            ["hidden"] = false
        }; 

    private static IDictionary<string,object?> _hide(Collection collection, Category? category) =>
        new Dictionary<string,object?> {
            ["id"] = collection.ID,
            ["name"] = "???",
            ["description"] = null,
            ["isMonthlyService"] = collection.is_monthly_service,
            ["monthlyService"] = collection.is_monthly_service ? _hide_monthly_service(collection,category) : null,
            ["hidden"] = true
        }; 





    private static IDictionary<string,object?> _show_monthly_service(Collection collection, Category? category) =>
        new Dictionary<string,object?> {
            ["category"] = category != null ? CategoryResponse.ToJson(category!) : null,
            ["moneyAmount"] = collection.money_amount,
            ["active"] = collection.is_monthly_service_active,
            ["hidden"] = false
        }; 

    private static IDictionary<string,object?> _hide_monthly_service(Collection collection, Category? category) =>
        new Dictionary<string,object?> {
            ["category"] = category != null ? CategoryResponse.ToJson(category,false) : null,
            ["moneyAmount"] = null,
            ["active"] = collection.is_monthly_service_active,
            ["hidden"] = true
        }; 


}