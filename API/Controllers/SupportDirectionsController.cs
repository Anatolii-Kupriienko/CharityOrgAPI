using API.Models;
using API.ServiceErrors;
using API.Services.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SupportDirectionsController(ICRUDService _CRUDService) : ApiController
    {
        private readonly ICRUDService _CRUDService = _CRUDService;
        public readonly string DeleteQuery = @"delete from supportDirections where id = @Id";
        public readonly string UpdateQuery = @"update supportDirections set name = @Name, description = @Description, about = @About where id = @Id";
        public readonly string InsertQuery = @"insert into supportDirections(name, description, about)values(@Name, @Description, @About)";
        public readonly string SelectQuery = @"select * from supportDirections";
        private readonly string GetIdContidion = @" where name = @Name and description = @Description";
        private readonly string GetOneCondition = @" where id = @Id";

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetSupportDirection(int id = 0)
        {
            string query = SelectQuery;
            if (id != 0)
                query += GetOneCondition;
            var responseResult = _CRUDService.Get<Employee>(query, id);
            return responseResult.Match(response => Ok(response), errors => Problem(errors));
        }

        [HttpPost]
        public IActionResult CreateSupportDirection(UpsertSupportDirectionRequest requestData)
        {
            var mapResult = MapRequestToModel(requestData);
            if (mapResult.IsError)
                return Problem(mapResult.Errors);
            var createResult = _CRUDService.Create(InsertQuery, mapResult.Value);
            if (createResult.IsError)
                return Problem(createResult.Errors);
            var id = _CRUDService.GetByData(SelectQuery + GetIdContidion, mapResult.Value).Value.Id;
            return CreatedAtAction(nameof(GetSupportDirection), new { Id = id }, new { });
        }

        [HttpPut]
        public IActionResult UpdateSupportDirection(UpsertSupportDirectionRequest requestData)
        {
            if (requestData.Id is null or < 1)
                return Problem(new List<Error> { Errors.General.InvalidId });

            var mapResult = MapRequestToModel(requestData);

            if (mapResult.IsError)
                return Problem(mapResult.Errors);

            var updateResult = _CRUDService.Update(UpdateQuery, mapResult.Value);
            return updateResult.Match(response => NoContent(), errors => Problem(errors));
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteSupportDirection(int id)
        {
            var deleteResult = _CRUDService.Delete(DeleteQuery, id);
            return deleteResult.Match(response => NoContent(), errors => Problem(errors));
        }
        private ErrorOr<SupportDirection> MapRequestToModel(UpsertSupportDirectionRequest requestData)
        {
            var requestToDataResult = SupportDirection.Create(requestData.Name, requestData.Description, requestData.About, requestData.Id);
            if (requestToDataResult.IsError)
                return requestToDataResult.Errors;
            return requestToDataResult.Value;
        }
    }
}
