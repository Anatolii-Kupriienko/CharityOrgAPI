using API.Models;
using API.Services;
using API.Services.Interfaces;
using API.Services.Reports;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<IReportsService, ReportsService>();
    builder.Services.AddScoped<ICUDService, CUDService>();
    builder.Services.AddScoped<ISimpleCRUDService, SimpleCRUDService>();
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();  
    app.MapControllers();
    app.Run();
}