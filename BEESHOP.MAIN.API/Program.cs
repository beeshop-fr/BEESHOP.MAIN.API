using BEESHOP.MAIN.API.Abstractions;
using BEESHOP.MAIN.PERSISTENCE;
using Npgsql;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Lancement de l'API");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointDefinitions(typeof(Program));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)); builder.Services.AddMediatR(cfg =>
{
    var assemblies = AppDomain.CurrentDomain
        .GetAssemblies()
        .Where(a => !a.IsDynamic && a.FullName!.StartsWith("BEESHOP"))
        .ToArray();

    cfg.RegisterServicesFromAssemblies(assemblies);
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<NpgsqlDataSource>();

try
{
    await using var conn = await db.OpenConnectionAsync();
    await using var cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT 1";
    var result = await cmd.ExecuteScalarAsync();
    Console.WriteLine($"PostgreSQL test réussi : {result}");
}
catch (Exception ex)
{
    Console.WriteLine($"Erreur lors du test de connexion PostgreSQL : {ex.Message}");
    throw; // Optionnel : tu peux le relancer pour planter au démarrage
}

app.UseSwagger();
app.UseSwaggerUI();

//app.UseAuthorization();
app.MapControllers();
app.UseEndpointDefinitions();

await app.RunAsync();
