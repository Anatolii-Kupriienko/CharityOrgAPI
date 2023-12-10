using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateEmployee(CreateEmployeeRequest request)
        {
            return Ok(request);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetEmployee(int id)
        {
            return Ok(id);
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateEmployee(int id, UpdateEmployeeRequest request)
        {
            return Ok(request);
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteEmployee(int id)
        {
            return Ok(id);
        }
    }
}
