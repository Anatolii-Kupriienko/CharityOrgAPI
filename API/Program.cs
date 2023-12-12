using API.Services.Employees;
using API.Services.SupportDirections;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
    builder.Services.AddSingleton<ISupportDirectionService, SupportDirectionService>();
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();  
    app.MapControllers();
    app.Run();
}