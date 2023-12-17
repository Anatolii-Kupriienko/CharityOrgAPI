using API.Models;
using API.ServiceErrors;
using API.Services.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProjectsController(ICRUDService _CRUDService) : ApiController
    {
        private readonly ICRUDService _CRUDService = _CRUDService;
        private readonly string SelectQuery = @"select * from projects";
        private readonly string GetIdCondition = @" where name = @Name and totalPrice = @TotalPrice and startDate = @StartDate";
        private readonly string GetOneCondition = @" where id = @Id";
        private readonly string InsertQuery = @"insert into projects(name, totalPrice, startDate, finishDate, link, isWithPartners, isMilitary, totalCollectedFunds)values(@Name, @TotalPrice, @StartDate, @FinishDate, @Link, @IsWithPartners, @IsMilitary, @TotalCollectedFunds)";
        private readonly string DeleteQuery = @"delete from projects where id = @Id";
        private readonly string UpdateQuery = @"update projects set name = @Name, totalPrice = @TotalPrice, startDate = @StartDate, finishDate = @FinishDate, link = @Link, isWithPartners = @IsWithPartners, isMilitary = @IsMilitary, totalCollectedFunds = @TotalCollectedFunds where id = @Id";

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetProject(int id = 0)
        {
            string query = SelectQuery;
            if (id != 0)
                query += GetOneCondition;
            var responseResult = _CRUDService.Get<Project>(query, id);
            return responseResult.Match(response => Ok(response), errors => Problem(errors));
        }

        [HttpPost]
        public IActionResult CreateProject(CreateProjectRequest requestData)
        {
            var mapResult = Project.Create(null, requestData.Name, requestData.TotalPrice, requestData.StartDate, null, requestData.Link, requestData.IsWithPartners, requestData.IsMilitary, null);
            if (mapResult.IsError)
                return Problem(mapResult.Errors);
            var createResult = _CRUDService.Create(InsertQuery, mapResult.Value);
            if (createResult.IsError)
                return Problem(createResult.Errors);
            var id = _CRUDService.GetByData(SelectQuery + GetIdCondition, mapResult.Value).Value.Id;
            return CreatedAtAction(nameof(GetProject), new {Id = id}, new { });
        }

        [HttpPut]
        public IActionResult UpdateProject(UpdateProjectRequest requestData)
        {
            if (requestData.Id < 1)
                return Problem(new List<Error> { Errors.SupportDirection.InvalidId });

            var mapResult = Project.Create(requestData.Id, requestData.Name, requestData.TotalPrice, requestData.StartDate, requestData.FinishDate, requestData.Link, requestData.IsWithPartners, requestData.IsMilitary, requestData.TotalFundsCollected);

            if (mapResult.IsError)
                return Problem(mapResult.Errors);

            var updateResult = _CRUDService.Update(UpdateQuery, mapResult.Value);
            return updateResult.Match(response => Ok(), errors => Problem(errors));
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteProject(int id)
        {
            var deleteResult = _CRUDService.Delete(DeleteQuery, id);
            return deleteResult.Match(response => Ok(), errors => Problem(errors));
        }
    }
}
