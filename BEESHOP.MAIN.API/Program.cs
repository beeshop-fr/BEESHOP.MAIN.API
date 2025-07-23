using BEESHOP.MAIN.API.Abstractions;
using BEESHOP.MAIN.PERSISTENCE;

using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Lancement de l'API");

var builder = WebApplication.CreateBuilder(args);

// Ajout du service Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointDefinitions(typeof(Program));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseAuthorization();
app.MapControllers();
app.UseEndpointDefinitions();

await app.RunAsync();
