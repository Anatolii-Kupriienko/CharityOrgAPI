using API.Models;
using API.ServiceErrors;
using API.Services.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SupportDirectionsController : ApiController
    {
        public SupportDirectionsController(ICRUDService CRUDService) : base(CRUDService)
        {
            _CRUDService = CRUDService;
        }
        private readonly ICRUDService _CRUDService;
        public readonly string DeleteQuery = @"delete from supportDirections where id = @Id";
        public readonly string UpdateQuery = @"update supportDirections set name = @Name, description = @Description, about = @About where id = @Id";
        public readonly string InsertQuery = @"insert into supportDirections(name, description, about)values(@Name, @Description, @About)";
        public readonly string SelectQuery = @"select * from supportDirections";
        private readonly string GetIdCondition = @" where name = @Name and description = @Description";
        private readonly string GetOneCondition = @" where id = @Id";

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetSupportDirection(int id = 0)
        {
            string query = SelectQuery;
            if (id != 0)
                query += GetOneCondition;

            return Get<SupportDirection>(query, id);
        }

        [HttpPost]
        public IActionResult CreateSupportDirection(UpsertSupportDirectionRequest requestData)
        {
            var mapResult = MapRequestToModel(requestData);
            if (mapResult.IsError)
                return Problem(mapResult.Errors);
            return Create(mapResult.Value, InsertQuery, SelectQuery + GetIdCondition, nameof(GetSupportDirection));
        }

        [HttpPut]
        public IActionResult UpdateSupportDirection(UpsertSupportDirectionRequest requestData)
        {
            if (requestData.Id is null or < 1)
                return Problem(new List<Error> { Errors.General.InvalidId });

            var mapResult = MapRequestToModel(requestData);

            if (mapResult.IsError)
                return Problem(mapResult.Errors);

            return Update(UpdateQuery, mapResult.Value);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteSupportDirection(int id)
        {
            return Delete(DeleteQuery, id);
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
