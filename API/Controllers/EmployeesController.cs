using API.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using API.ServiceErrors;
using ErrorOr;
using API.Services.Interfaces;

namespace API.Controllers
{
    public class EmployeesController : ApiController
    {
        public EmployeesController(ICRUDService CRUDService) : base(CRUDService) { }
        private readonly string SelectQuery = @"select * from employees";
        private readonly string GetIdCondition = @" where firstName = @FirstName and lastName = @LastName and birthDate = @BirthDate and startWorkDate = @StartWorkDate and Position = @Position;";
        private readonly string GetOneCondition = @" where id = @Id";
        private readonly string InsertQuery = @"insert into employees(firstName, lastName, birthDate, startWorkDate, Position) values(@FirstName, @LastName, @BirthDate, @StartWorkDate, @Position)";
        private readonly string DeleteQuery = @"delete from employees where id = @Id";
        private readonly string UpdateQuery = @"update employees set Position = @Position where id = @Id";

        [HttpPost]
        public IActionResult CreateEmployee(CreateEmployeeRequest request)
        {
            var requestToEmployeeResult = Employee.Create(request.FirstName, request.LastName, request.BirthDate, request.StartWorkDate, request.Position);
            
            if (requestToEmployeeResult.IsError)
                return Problem(requestToEmployeeResult.Errors);

            return Create(requestToEmployeeResult.Value, InsertQuery, SelectQuery + GetIdCondition, nameof(GetEmployee));
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetEmployee(int id = 0)
        {
            string query = SelectQuery;
            if (id != 0)
                query += GetOneCondition;

            return Get<Employee>(query, new {Id = id});
        }
        
        [HttpPut]
        public IActionResult UpdateEmployee(UpdateEmployeeRequest request)
        {
            return Update(UpdateQuery, request);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteEmployee(int id)
        {
            return Delete(DeleteQuery, new { Id = id });
        }
    }
}
