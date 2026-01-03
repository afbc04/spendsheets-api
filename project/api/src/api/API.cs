using Controller;
using Serilog;
using DAO;
using System.Threading.Tasks;

public class API {

    public static Manager? _manager {get; private set;}

    public static Manager GetAPI() {
        return API._manager!;
    }

    public static async Task<bool> StartAPI() {

        try {

            ProgramHandler.StartLogger();
            PacketTemplates.TemplateLoader.LoadTemplates();
            await DAOManager.StartDatabase();
            API._manager = await Manager.Start();

            return true;

        }
        catch (ControllerManagerException ex) {
            Log.Error("Could not load API");
            Log.Error(ex.StackTrace!);
            return false;
        }
        catch (Exception ex) {
            Log.Error(ex.StackTrace!);
            return false;
        }

    }

}