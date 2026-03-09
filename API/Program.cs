using Application.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Seeders;
using Serilog;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;

builder.AddPresentation();
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(config);

var app = builder.Build();

// Add seeder to the database
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
    await seeder.Seed();
}

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
