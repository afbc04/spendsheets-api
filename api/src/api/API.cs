using Controller;
using Serilog;
using DAO;
using ConfigHandler;
using Manager;

public class API {

    public static APIManager? _manager {get; private set;}

    public static APIManager GetAPI() {
        return API._manager!;
    }

    public static async Task<bool> StartAPI() {

        try {

            ProgramHandler.StartLogger();

            Config config = Config.Get();

            //PacketTemplates.TemplateLoader.LoadTemplates();
            await DAOManager.StartDatabase();
            API._manager = await APIManager.Start();

            config.UpdateInstance();
            if(Config.Save())
                Log.Information("Config was saved");
            else
                Log.Error("Config could not be saved");

            return true;

        }
        catch (Exception ex) {
            Log.Error(ex.StackTrace!);
            return false;
        }

    }

}