public static class Routers {

    public static void Register(WebApplication app) {
        
        var api = app.MapGroup("");

        api.RecordRoutersMapping();
        api.TagRoutersMapping();
        api.CategoryRoutersMapping();
        api.UserRoutersMapping();
        api.ProfileRoutersMapping();
        api.SessionRoutersMapping();
        api.TokenRoutersMapping();

    }

}
