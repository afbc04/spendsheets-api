public static class v1Routers {

    public static void Register(WebApplication app) {
        
        var api = app.MapGroup("/v1.0");

        api.RecordRoutersMapping();
        api.TagRoutersMapping();
        api.CategoryRoutersMapping();
        api.UserRoutersMapping();
        api.TokenRoutersMapping();

    }

}
