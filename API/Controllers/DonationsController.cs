using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DonationsController(ICRUDService _CRUDService) : ApiController
    {
        private readonly ICRUDService _CRUDService = _CRUDService;
        private readonly string SelectQuery = @"select * from donations";
        private readonly string GetOneCondition = @" where donations.id = @Id";
        private readonly string GetIdCondition = @" where sender = @Sender and date = @Date and amount = @Amount and currency = @Currency";
        private readonly string Join = @" left join supportDirections on supportDirections.id = supportDirectionId";
        private readonly string InsertQuery = @"insert into donations(sender, amount, currency, supportDirectionId, date)values(@Sender, @Amount, @Currency, @SupportDirectionId, @Date)";
        private readonly string UpdateQuery = @"update donations set sender = @Sender, amount = @Amount, currency = @Currency, supportDirectionId = @SupportDirectionId, date = @Date where id = @Id";
        private readonly string DeleteQuery = @"delete from donations where id = @Id";

        [HttpPost]
        public IActionResult Create(DonationsRequest requestData)
        {
            var validationResult = Donation.ValidateDonation(requestData);
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
            var getResult = _CRUDService.Get<Donation, SupportDirection>(query, id, Donation.MapQuery);
            return getResult.Match((result) => Ok(Donation.MapModel(result)), errors => Problem(errors));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var deleteResult = _CRUDService.Delete(DeleteQuery, id);
            return deleteResult.Match(result => Ok(), errors => Problem(errors));
        }

        [HttpPut]
        public IActionResult Update(DonationsRequest requestData)
        {
            var validationResult = Donation.ValidateDonation(requestData);
            if (validationResult.IsError)
                return Problem(validationResult.Errors);
            var updateResult = _CRUDService.Update(UpdateQuery, requestData);
            return updateResult.Match(result => Ok(), errors => Problem(errors));
        }
    }
}
