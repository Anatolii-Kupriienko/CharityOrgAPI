using API.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using API.Services.Employees;
using API.ServiceErrors;
using ErrorOr;

namespace API.Controllers
{
    public class EmployeesController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateEmployee(CreateEmployeeRequest request)
        {
            DateTime birthDate, startWorkDate;
            if(!DateTime.TryParse(request.BirthDate, out birthDate) || !DateTime.TryParse(request.StartWorkDate, out startWorkDate))
                return Problem(new List<Error> { Errors.Employee.InvalidDate });

            var requestToEmployeeResult = Employee.Create(request.FirstName, request.LastName, birthDate, startWorkDate, request.Position);
            
            if (requestToEmployeeResult.IsError)
                return Problem(requestToEmployeeResult.Errors);

            _employeeService.CreateEmployee(requestToEmployeeResult.Value);
            var id = EmployeeService.GetEmployeeId(requestToEmployeeResult.Value);
            return CreatedAtAction(nameof(GetEmployee), new {Id = id }, new {});
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetEmployee(int id = 0)
        {
            var responseResult = _employeeService.GetEmployee(id);
            return responseResult.Match(response => Ok(response), errors => Problem(errors));
        }
        
        [HttpPut]
        public IActionResult UpdateEmployee(UpdateEmployeeRequest request)
        {
            var result = _employeeService.UpdateEmployee(request);
            return result.Match(result => NoContent(), errors => Problem(errors));
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteEmployee(int id)
        {
            var responseResult = _employeeService.DeleteEmployee(id);
            return responseResult.Match(result => NoContent(), errors => Problem(errors));
        }
    }
}
