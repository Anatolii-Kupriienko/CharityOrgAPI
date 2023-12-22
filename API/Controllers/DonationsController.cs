using API.Models;
using API.Services.Interfaces;
using Kursova.Contracts.Donations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DonationsController(ICRUDService CRUDService) : ApiController(CRUDService)
    {
        private readonly string SelectQuery = @"select * from donations";
        private readonly string GetOneCondition = @" where donations.id = @Id";
        private readonly string SelectIdQuery = @"select id from donations where sender = @Sender and date = @Date and amount = @Amount and currency = @Currency";
        private readonly string Join = @" left join supportDirections on supportDirections.id = supportDirectionId";
        private readonly string InsertQuery = @"insert into donations(sender, amount, currency, supportDirectionId, date)values(@Sender, @Amount, @Currency, @SupportDirectionId, @Date)";
        private readonly string UpdateQuery = @"update donations set sender = @Sender, amount = @Amount, currency = @Currency, supportDirectionId = @SupportDirectionId, date = @Date where id = @Id";
        private readonly string DeleteQuery = @"delete from donations where id = @Id";
        private readonly string GetSupportDirectionCondition = @" where supportDirectionId = @Id";
        private readonly string GetByCurrencyCondition = @" where currency = @Currency";
        private readonly string GetBetweenDatesCondition = @" where date between @StartDate and @EndDate";
        private readonly string LikeNameCondition = @" where sender like @Name";

        [HttpPost]
        public IActionResult Create(DonationsRequest requestData)
        {
            var validationResult = Donation.ValidateDonation(requestData);
            if (validationResult.IsError)
                return Problem(validationResult.Errors);
            return Create(requestData, InsertQuery, SelectIdQuery, nameof(GetDonation));
            
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetDonation(int id = 0)
        {
            string query = SelectQuery + Join;
            if (id != 0)
                query += GetOneCondition;
            return Get<Donation, SupportDirection, DonationsResponse>(query, new {Id = id}, Donation.MapQuery, Donation.MapModel);
        }

        [HttpGet("supportDirection")]
        [HttpGet("supportDirection/{id:int}")]
        public IActionResult GetDonationsForSupportDirection(int id = 0)
        {
            string query = SelectQuery + Join;
            if(id != 0)
                query += GetSupportDirectionCondition;
            return Get<Donation, SupportDirection, DonationsForSupportDirectionResponse>
                (query, new { Id = id }, Donation.MapQuery, Donation.MapFilteredModel);
        }

        [HttpGet("orderBy")]
        [HttpGet("orderBy/{startDate:datetime}/{endDate:datetime}")]
        public IActionResult GetSortedBetweenDates(OrderByRequest orderByRequest,DateTime? startDate, DateTime? endDate)
        {
            string query = SelectQuery + Join;
            if(startDate != null && endDate != null)
                query += GetBetweenDatesCondition;
            query += orderByRequest.GetOrderByStatement();
            return Get<Donation, SupportDirection, DonationsResponse>
                (query, new { StartDate = startDate, EndDate = endDate }, Donation.MapQuery, Donation.MapModel);
        }

        [HttpGet("currency/{currency}")]
        public IActionResult GetByCurrency(string currency)
        {
            string query = SelectQuery + Join + GetByCurrencyCondition;
            return Get<Donation, SupportDirection, DonationsResponse>
                (query, new { Currency = currency }, Donation.MapQuery, Donation.MapModel);
        }

        [HttpGet("sender/{name}")]
        public IActionResult GetByName(string name)
        {
            string query = SelectQuery + Join + LikeNameCondition;
            return Get<Donation, SupportDirection, DonationsResponse>
                (query, new { Name = $"%{name}%" }, Donation.MapQuery, Donation.MapModel);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteDonation(int id)
        {
            return Delete(DeleteQuery, new {Id = id});
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
