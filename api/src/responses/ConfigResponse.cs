using ConfigHandler;

public static class ConfigResponse {

    public static IDictionary<string,object?> ToJson(Config config) =>
        new Dictionary<string,object?> {
            ["databaseVersion"] = config.database_version,
            ["lastOnlineDate"] = Utils.ConvertToDateTime(config.last_online_date),
            ["name"] = config.name,
            ["public"] = config.is_public
        };


}