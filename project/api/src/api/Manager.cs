using Controller;

public class Manager {

    public ConfigManager Config {private set; get;}
    public TokenManager Token {private set; get;}
    public TagManager Tag {private set; get;}
    public CategoryManager Category {private set; get;}
    public MonthlyServiceManager MonthlyService {private set; get;}

    private Manager(ConfigController config, TokenController token, TagController tag, CategoryController category, MonthlyServiceController monthly_service) {

        this.Config = new ConfigManager(config,token);
        this.Token = new TokenManager(token,config);
        this.Tag = new TagManager(tag,token,config);
        this.Category = new CategoryManager(category,token,config);
        this.MonthlyService = new MonthlyServiceManager(monthly_service,category,token,config);

    }

    public static async Task<Manager> Start() {

        //Create controllers
        var config_controller = new ConfigController();
        var token_controller = new TokenController();
        var tag_controller = new TagController();
        var category_controller = new CategoryController();
        var monthly_service_controller= new MonthlyServiceController();

        // Load controllers
        await config_controller._Load();



        await config_controller._Finish();

        // Create manager
        return new Manager(config_controller,token_controller,tag_controller,category_controller,monthly_service_controller);

    }

}


