using Serilog;

public class Program {

    public static async Task Main(string[] args)
    {
        if (!await StartApi())
            return;

        Log.Information($"API is listenning at port {Environment.GetEnvironmentVariable("API_PORT")}");

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Frontend", policy =>
            {
                policy
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        var app = builder.Build();
        app.UseMiddleware<ErrorMiddleware>();

        app.UseCors("Frontend"); // 🔥 TEM de vir aqui
        app.Use(ProgramHandler.LogRequests);
        Routers.Register(app);
        DocsRouter.Register(app);

        app.Run();
    }

    private static async Task<bool> StartApi()
    {
        try
        {
            ProgramHandler.StartLogger();
            DatabaseStatus databaseStatus = await DatabaseManager.LinkWithDatabase();
            await SessionManager.InitSessionManager();
            return true;
            //LogDatabaseStatus(databaseStatus);
            //return databaseStatus == DatabaseStatus.Success;
        }
        catch (Exception ex) {
            Log.Error(ex.StackTrace!);
            return false;
        }
    }

    private static void LogDatabaseStatus(DatabaseStatus status)
    {
        switch (status)
        {
            case DatabaseStatus.Success:
                Log.Information("Database connected successfully");
                break;

            case DatabaseStatus.Exception:
                Log.Error("Database startup had an error");
                break;

            case DatabaseStatus.ConnectionFail:
                Log.Error("Couldn't connect with database");
                break;

            case DatabaseStatus.OlderVersion:
                Log.Error("API is not compatible with database. Its too old");
                break;

            case DatabaseStatus.NewerVersion:
                Log.Warning("API possibly is not compatible with newer databases. Try a newer API");
                break;

            case DatabaseStatus.SetupFail:
                Log.Error("Error while configurating database");
                break;

            case DatabaseStatus.Corrupted:
                Log.Error("Database is corrupted or damaged");
                break;

            default:
                Log.Warning("Unknown database status");
                break;
        }
    }
}
