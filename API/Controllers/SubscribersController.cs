using API.Models;
using API.ServiceErrors;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace API.Controllers
{
    public class SubscribersController : ApiController
    {
        public SubscribersController(ICRUDService CRUDService) : base(CRUDService){}
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

            return Create(requestData, InsertQuery, SelectQuery + GetIdCondition, nameof(GetSub));
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetSub(int id = 0)
        {
            string query = SelectQuery + Join;
            if (id != 0)
                query += GetOneCondition;

            return Get<Subscriber, SupportDirection, SubscribersResponse>
                (query, new { Id = id }, Subscriber.MapQuery, Subscriber.MapModel);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteSub(int id)
        {
            return Delete(DeleteQuery, new { Id = id });
        }

        [HttpPut]
        public IActionResult UpdateSub(SubscribersRequest requestData)
        {
            if (requestData.Id is null or < 0)
               return Problem([Errors.General.InvalidId]);

            var validationResult = Subscriber.Validate(requestData);
            if (validationResult.IsError)
                return Problem(validationResult.Errors);

            return Update(UpdateQuery, requestData);
        }
    }
}
