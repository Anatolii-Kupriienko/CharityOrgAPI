using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DonationsController : ApiController
    {
        public DonationsController(ICRUDService CRUDService) : base(CRUDService)
        {
            _CRUDService = CRUDService;
        }

        private readonly ICRUDService _CRUDService;
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
            return Create(validationResult.Value, InsertQuery, SelectQuery + GetIdCondition, nameof(GetDonation));
            
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetDonation(int id = 0)
        {
            string query = SelectQuery + Join;
            if (id != 0)
                query += GetOneCondition;
            return Get<Donation, SupportDirection, DonationsResponse>(query, id, Donation.MapQuery, Donation.MapModel);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteDonation(int id)
        {
            return Delete(DeleteQuery, id);
        }

        [HttpPut]
        public IActionResult UpdateDonation(DonationsRequest requestData)
        {
            var validationResult = Donation.ValidateDonation(requestData);
            if (validationResult.IsError)
                return Problem(validationResult.Errors);

            return Update(UpdateQuery, requestData);
        }
    }
}
