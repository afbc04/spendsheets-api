using System.Threading.Tasks;
using Serilog;

public class Program {

    public static async Task Main(string[] args) {

        if (await API.StartAPI() == true) {

            Log.Information("API was successfully started");

            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.Use(ProgramHandler.LogRequests);
            v1Routers.Register(app);
            DocsRouter.Register(app);

            app.Run();

        }
        else {
            Log.Error("API couldn't start");
        }

    }

}
