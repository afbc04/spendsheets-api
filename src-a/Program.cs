using Microsoft.EntityFrameworkCore;
using TodoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// ---- Services ----
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=Data/app.db";

// Garante que a pasta onde fica o ficheiro .db existe (o SQLite não a cria sozinho)
var dbPath = connectionString.Replace("Data Source=", "", StringComparison.OrdinalIgnoreCase).Trim();
var dbDir = Path.GetDirectoryName(dbPath);
if (!string.IsNullOrWhiteSpace(dbDir))
{
    Directory.CreateDirectory(dbDir);
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// ---- Aplica as EF Core Migrations automaticamente no arranque ----
// (cria a BD se não existir e aplica quaisquer migrations pendentes)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ---- Middleware ----
// Swagger fica sempre disponível (mesmo em "Production"), porque é uma API
// e o docker-compose de produção corre com ASPNETCORE_ENVIRONMENT=Production.
// Se preferires esconder o Swagger em produção, volta a colocar isto dentro
// de "if (app.Environment.IsDevelopment()) { ... }".
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
