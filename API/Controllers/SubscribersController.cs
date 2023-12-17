using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace API.Controllers
{
    public class SubscribersController(ICRUDService _CRUDService) : ApiController
    {
        private readonly ICRUDService _CRUDService = _CRUDService;
        private readonly string SelectQuery = @"select * from subscribers";
        private readonly string GetOneCondition = @" where subscribers.id = @Id";
        private readonly string GetIdCondition = @" where fullName = @FullName and dateSubscribed = @DateSubscribed and currency = @Currency and amount = @Amount";
        private readonly string Join = @" left join supportDirections on supportDirections.id = supportDirectionId";
        private readonly string UpdateQuery = @"update subscribers set fullName = @FullName, amount = @Amount, currency = @Currency, dateSubscribed = @DateSubscribed, supportDirectionId = @SupportDirectionId where id = @Id";
        private readonly string DeleteQuery = @"delete from subscribers where id = @Id";
        private readonly string InsertQuery = @"insert into subscribers(fullName, amount, currency, dateSubscribed, supportDirectionId)values(@FullName, @Amount, @Currency, @DateSubscribed, @SupportDirectionId)";

        [HttpPost]
        public IActionResult Create(SubscribersRequest requestData)
        {
            var validationResult = Subscriber.Validate(requestData);
            if (validationResult.IsError)
                return Problem(validationResult.Errors);
            var createResult = _CRUDService.Create(InsertQuery, requestData);
            if (createResult.IsError)
                return Problem(createResult.Errors);
            var id = _CRUDService.GetByData(SelectQuery + GetIdCondition, requestData).Value.Id;
            return CreatedAtAction(nameof(Get), new { Id = id }, new { });
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult Get(int id = 0)
        {
            string query = SelectQuery + Join;
            if (id != 0)
                query += GetOneCondition;
            var getResult = _CRUDService.Get<Subscriber, SupportDirection>(query, id, Subscriber.MapQuery);
            return getResult.Match((result) => Ok(Subscriber.MapModel(result)), errors => Problem(errors));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var deleteResult = _CRUDService.Delete(DeleteQuery, id);
            return deleteResult.Match(result => Ok(), errors => Problem(errors));
        }

        [HttpPut]
        public IActionResult Update(SubscribersRequest requestData)
        {
            var validationResult = Subscriber.Validate(requestData);
            if (validationResult.IsError)
                return Problem(validationResult.Errors);
            var updateResult = _CRUDService.Update(UpdateQuery, requestData);
            return updateResult.Match(result => Ok(), errors => Problem(errors));
        }
    }
}
