using System.Threading.Tasks;
using Serilog;

public class Program {

    public static async Task Main(string[] args) {

        if (await API.init() == true) {

            Log.Information("API was successfully started");

            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls("http://*:25000");

            var app = builder.Build();
            app.Use(ProgramHandler.log_requests);
            v1Routers.Register(app);
            DocsRouter.Register(app);

            app.Run();

        }
        else {
            Log.Error("API couldn't start");
        }

    }

}
