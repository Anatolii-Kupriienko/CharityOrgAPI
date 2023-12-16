using API.Services;
using API.Services.Employees;
using API.Services.Projects;
using API.Services.SupportDirections;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<IEmployeeService, EmployeeService>();
    builder.Services.AddScoped<ISupportDirectionService, SupportDirectionService>();
    builder.Services.AddScoped<IProjectService, ProjectService>();
    //builder.Services.AddScoped<IService, EmployeeService>(); 
}

var app = builder.Build();
{
    
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();  
    app.MapControllers();
    app.Run();
}