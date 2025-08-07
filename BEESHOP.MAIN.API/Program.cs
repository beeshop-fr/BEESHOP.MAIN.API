using BEESHOP.MAIN.API.Abstractions;
using BEESHOP.MAIN.APPLICATION;
using BEESHOP.MAIN.PERSISTENCE;
using Npgsql;
using Serilog;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Lancement de l'API");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:3000") // <- ton front Nuxt
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); ;
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
    throw;
}
app.UseCors();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseEndpointDefinitions();

await app.RunAsync();
