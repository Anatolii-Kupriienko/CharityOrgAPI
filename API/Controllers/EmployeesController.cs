using API.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using API.Services.Employees;

namespace API.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateEmployee(CreateEmployeeRequest request)
        {
            var response = new EmployeeResponse(0, request.FirstName, request.LastName, request.BirthDate, request.StartWorkDate, request.Position);
            response = _employeeService.CreateEmployee(response);
            return CreatedAtAction(nameof(GetEmployee), new {id = response.Id},response);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetEmployee(int id = 0)
        {
            var response = _employeeService.GetEmployee(id);
            return Ok(response);
        }
        
        [HttpPut]
        public IActionResult UpdateEmployee(UpdateEmployeeRequest request)
        {
             _employeeService.UpdateEmployee(request);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteEmployee(int id)
        {
            _employeeService.DeleteEmployee(id);
            return NoContent();
        }
    }
}
