public static class Routers {

    public static void Register(WebApplication app) {
        
        var api = app.MapGroup("");

        api.ProfileRoutersMapping();
        api.SessionRoutersMapping();
        api.WorkspacesRoutersMapping();
        api.RecordRoutersMapping();

    }

}
