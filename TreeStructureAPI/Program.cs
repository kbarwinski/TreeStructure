

using TreeStructureAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.ConfigureCORS();
builder.Services.ConfigureSQLContext(configuration);
builder.Services.ConfigureRepositories();
// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
    
app.MigrateDatabase();

app.Run();
