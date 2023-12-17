using API.Models;
using API.Services.Implementations;
using API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<ICRUDService, SimpleCRUDService>();
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();  
    app.MapControllers();
    app.Run();
}