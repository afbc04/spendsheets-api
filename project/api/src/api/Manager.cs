using Controller;

public class Manager {

    public ConfigManager Config {private set; get;}
    public TokenManager Token {private set; get;}
    public TagManager Tag {private set; get;}
    public CategoryManager Category {private set; get;}
    public MonthlyServiceManager MonthlyService {private set; get;}
    public EntryManager Entry {private set; get;}
    public EntryTagsManager EntryTags {private set; get;}
    public EntryNotesManager EntryNotes {private set; get;}
    public EntryMovementsManager EntryMovements {private set; get;}

    private Manager(ConfigController config, TokenController token, TagController tag, CategoryController category, MonthlyServiceController monthly_service, EntryController entry, EntryTagsController entry_tags, EntryNotesController entry_notes, EntryMovementsController entry_movements) {

        this.Config = new ConfigManager(config,token);
        this.Token = new TokenManager(token,config);
        this.Tag = new TagManager(tag,token,config);
        this.Category = new CategoryManager(category,token,config);
        this.MonthlyService = new MonthlyServiceManager(monthly_service,category,token,config);
        this.Entry = new EntryManager(entry,monthly_service,category,token,config);
        this.EntryTags = new EntryTagsManager(entry_tags,entry,tag,token,config);
        this.EntryNotes = new EntryNotesManager(entry_notes,entry,token,config);
        this.EntryMovements = new EntryMovementsManager(entry_movements,entry,token,config);

    }

    public static async Task<Manager> Start() {

        //Create controllers
        var config_controller = new ConfigController();
        var token_controller = new TokenController();
        var tag_controller = new TagController();
        var category_controller = new CategoryController();
        var monthly_service_controller = new MonthlyServiceController();
        var entry_controller = new EntryController();
        var entry_tags_controller = new EntryTagsController();
        var entry_notes_controller = new EntryNotesController();
        var entry_movements_controller = new EntryMovementsController();

        // Load controllers
        await config_controller._Load();



        await config_controller._Finish();

        // Create manager
        return new Manager(config_controller,token_controller,tag_controller,category_controller,monthly_service_controller,entry_controller,entry_tags_controller,entry_notes_controller,entry_movements_controller);

    }

}


