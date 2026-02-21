using Routers;

public static class v1Routers {

    public static void Register(WebApplication app) {
        
        var api = app.MapGroup("/v1.0");

        
        api.ConfigRoutersMapping();
        api.TokenRoutersMapping();
        api.ReaderTokenRoutersMapping();
        api.CategoryRoutersMapping();
        api.CollectionRoutersMapping();
        api.EntryRoutersMapping();

    }

}
