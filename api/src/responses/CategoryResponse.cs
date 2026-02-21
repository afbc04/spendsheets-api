public static class CategoryResponse {

    public static IDictionary<string,object?> ToJson(Category category) => _show(category);

    public static IDictionary<string,object?> ToJson(Category category, bool can_read) =>
        can_read ? _show(category) : _hide(category);


    private static IDictionary<string,object?> _show(Category category) =>
        new Dictionary<string,object?> {
            ["id"] = category.ID,
            ["name"] = category.name,
            ["description"] = category.description,
            ["hidden"] = false
        }; 

    private static IDictionary<string,object?> _hide(Category category) =>
        new Dictionary<string,object?> {
            ["id"] = category.ID,
            ["name"] = "???",
            ["description"] = null,
            ["hidden"] = true
        }; 


}