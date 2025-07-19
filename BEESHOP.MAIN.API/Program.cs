var builder = WebApplication.CreateBuilder(args);

// Ajout du service Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


var app = builder.Build();

//// Activation du middleware Swagger en développement
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseAuthorization();
app.MapControllers();

app.Run();
