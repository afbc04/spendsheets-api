public static class PageView
{
    public static Dictionary<string,object?> ToView<T>(QueryPage page, List<T> list, long totalElements)
    {
        long pageElements = list.Count;
        long totalPages = totalElements / page.Limit;
        if (totalElements > totalPages * page.Limit)
            totalPages++;

        if (totalElements < list.Count)
            totalElements = list.Count;

        return new Dictionary<string,object?>(){
            ["totalElements"] = totalElements,
            ["pageElements"] = list.Count,
            ["page"] = page.Page,
            ["limit"] = page.Limit,
            ["totalPages"] = totalPages,
            ["empty"] = pageElements == 0,
            ["all"] = pageElements == totalElements,
            ["firstPage"] = page.Page == 1,
            ["lastPage"] = page.Page == totalPages,
            ["data"] = list
        };
    }
}