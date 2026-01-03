using PacketHandlers;
using Nito.AsyncEx;
using Queries;
using Controller;

public class Manager {

    public ConfigManager Config {private set; get;}
    public TokenManager Token {private set; get;}
    public TagManager Tag {private set; get;}

    private Manager(ConfigController config, TokenController token, TagController tag) {

        this.Config = new ConfigManager(config,token);
        this.Token = new TokenManager(token,config);
        this.Tag = new TagManager(tag,token,config);

    }

    public static async Task<Manager> Start() {

        //Create controllers
        var config_controller = new ConfigController();
        var token_controller = new TokenController();
        var tag_controller = new TagController();

        // Load controllers
        await config_controller._Load();



        await config_controller._Finish();

        // Create manager
        return new Manager(config_controller,token_controller,tag_controller);

    }

}


