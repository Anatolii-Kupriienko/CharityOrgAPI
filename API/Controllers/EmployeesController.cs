using API.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using API.ServiceErrors;
using ErrorOr;
using API.Services.Interfaces;

namespace API.Controllers
{
    public class EmployeesController(ISimpleCRUDService _CRUDService) : ApiController
    {
        private readonly ISimpleCRUDService _CRUDService = _CRUDService;
        private readonly string SelectQuery = @"select * from employees";
        private readonly string GetIdCondition = @" where firstName = @FirstName and lastName = @LastName and birthDate = @BirthDate and startWorkDate = @StartWorkDate and Position = @Position;";
        private readonly string GetOneCondition = @" where id = @Id";
        private readonly string InsertQuery = @"insert into employees(firstName, lastName, birthDate, startWorkDate, Position) values(@FirstName, @LastName, @BirthDate, @StartWorkDate, @Position)";
        private readonly string DeleteQuery = @"delete from employees where id = @Id";
        private readonly string UpdateQuery = @"update employees set Position = @Position where id = @Id";

        [HttpPost]
        public IActionResult CreateEmployee(CreateEmployeeRequest request)
        {
            DateTime birthDate, startWorkDate;
            if(!DateTime.TryParse(request.BirthDate, out birthDate) || !DateTime.TryParse(request.StartWorkDate, out startWorkDate))
                return Problem(new List<Error> { Errors.Employee.InvalidDate });

            var requestToEmployeeResult = Employee.Create(request.FirstName, request.LastName, birthDate, startWorkDate, request.Position);
            
            if (requestToEmployeeResult.IsError)
                return Problem(requestToEmployeeResult.Errors);

            _CRUDService.Create(InsertQuery, requestToEmployeeResult.Value);
            var id = _CRUDService.GetByData(SelectQuery + GetIdCondition, requestToEmployeeResult.Value).Value.Id;
            return CreatedAtAction(nameof(GetEmployee), new {Id = id }, new {});
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetEmployee(int id = 0)
        {
            string query = SelectQuery;
            if (id != 0)
                query += GetOneCondition;
            var responseResult = _CRUDService.Get<Employee>(query, id);
            return responseResult.Match(response => Ok(response), errors => Problem(errors));
        }
        
        [HttpPut]
        public IActionResult UpdateEmployee(UpdateEmployeeRequest request)
        {
            var result = _CRUDService.Update(UpdateQuery, request);
            return result.Match(result => NoContent(), errors => Problem(errors));
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteEmployee(int id)
        {
            var responseResult = _CRUDService.Delete(DeleteQuery, id);
            return responseResult.Match(result => NoContent(), errors => Problem(errors));
        }
    }
}
