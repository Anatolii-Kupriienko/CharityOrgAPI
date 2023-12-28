using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace API.Controllers
{
    public class RequestsController(ICRUDService cRUDService) : ApiController(cRUDService)
    {
        private readonly string SelectQuery = @"select * from requests";
        private readonly string GetOneCondition = @" where requests.id = @Id";
        private readonly string SelectIdQuery = @"select id from requests where requester = @Requester and itemId = @ItemId";
        private readonly string OrderByDateCondition = @" order by dateRecieved ";
        private readonly string GetByRequesterCondition = @" where requester like @Requester";
        private readonly string GetBetweenDatesCondition = @" where dateRecieved between @StartDate and @EndDate";
        private readonly string JoinItems = @" left join suppliedItems on suppliedItems.id = itemId";
        private readonly string JoinReports = @" left join reports on reports.id = reportId";
        private readonly string InsertQuery = @"insert into requests(itemId, amount, dateRecieved, urgency, requester)values(@ItemId, @Amount, @DateRecieved, @Urgency, @Requester)";
        private readonly string DeleteQuery = @"delete from requests";
        private readonly string UpdateQuery = @"update requests set itemId = @ItemId, amount = @Amount, dateRecieved = @DateRecieved, urgency = @Urgency, requester = @Requester, reportId = @ReportId where id = @Id";
        private readonly string SetReportQuery = @"update requests set reportId = @ReportId where id = @Id";
        private readonly string GetByUrgencyCondition = @" where urgency like @Urgency";
        private readonly string SortByUrgency = @" order by urgency";
        private readonly string GetCompletedCondition = @" where reportId is not null";
        private readonly string GetNotCompletedCondition = @" where reportId is null";


        [HttpPost]
        public IActionResult CreateRequest(CreateRequestRequest requestData)
        {
            return Models.Request.Create(requestData).
                Match(result => Create(result, InsertQuery, SelectIdQuery, nameof(GetRequest))
                , Problem);
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetRequest(int id = 0)
        {
            string query = SelectQuery + JoinItems + JoinReports;
            if (id != 0)
                query += GetOneCondition;
            return Get<Request, SuppliedItem, Report, RequestsResponse>
                (query, new { Id = id }, Models.Request.MapQuery, Models.Request.MapModel);
        }

        [HttpGet("{startDate:datetime}/{endDate:datetime}")]
        public IActionResult GetRequestBetweenDates(DateTime startDate, DateTime endDate)
        {
            string query = SelectQuery + JoinItems + JoinReports + GetBetweenDatesCondition;
            return Get<Request, SuppliedItem, Report, RequestsResponse>
                (query, new { StartDate = startDate, EndDate = endDate }, Models.Request.MapQuery, Models.Request.MapModel);
        }

        [HttpGet("orderByDate/{startDate:datetime}/{endDate:datetime}/{desc:bool}")]
        [HttpGet("orderByDate/{startDate:datetime}/{endDate:datetime}")]
        public IActionResult GetRequestsBetweenDatesOrdered(DateTime startDate, DateTime endDate, bool desc = false)
        {
            string query = SelectQuery + JoinItems + JoinReports + GetBetweenDatesCondition + OrderByDateCondition;
            if (desc)
                query += "desc";
            return Get<Request, SuppliedItem, Report, RequestsResponse>
                (query, new { StartDate = startDate, EndDate = endDate }, Models.Request.MapQuery, Models.Request.MapModel);
        }

        [HttpGet("orderByDate")]
        [HttpGet("orderByDate/{desc:bool}")]
        public IActionResult GetRequestsOrderedByDate(bool desc = false)
        {
            string query = SelectQuery + JoinItems + JoinReports + OrderByDateCondition;
            if (desc)
                query += "desc";
            return Get<Request, SuppliedItem, Report, RequestsResponse>
                (query, new { }, Models.Request.MapQuery, Models.Request.MapModel);
        }


        [HttpGet("requester")]
        [HttpGet("requester/{requesterName}")]
        public IActionResult GetRequestsForRequester(string? requesterName)
        {
            string query = SelectQuery + JoinItems + JoinReports;
            if (requesterName != null)
                query += GetByRequesterCondition;
            return Get<Request, SuppliedItem, Report, AllRequestsByRequesterResponse>
                (query, new { Requester = $"%{requesterName}%" }, Models.Request.MapQuery, Models.Request.ModelFilteredModel);
        }

        [HttpGet("urgency")]
        [HttpGet("urgency/{urgency}")]
        public IActionResult GetRequestsForUrgency(string? urgency)
        {
            string query = SelectQuery + JoinItems + JoinReports;
            if (urgency != null)
                query += GetByUrgencyCondition;
            query += SortByUrgency;
            return Get<Request, SuppliedItem, Report, RequestsResponse>
                (query, new { Urgency = $"%{urgency}%" }, Models.Request.MapQuery, Models.Request.MapModel);
        }

        [HttpGet("completed")]
        public IActionResult GetCompletedRequests()
        {
            string query = SelectQuery + JoinItems + JoinReports + GetCompletedCondition;
            return Get<Request, SuppliedItem, Report, RequestsResponse>
                (query, new { }, Models.Request.MapQuery, Models.Request.MapModel);
        }

        [HttpGet("notCompleted")]
        [HttpGet("notCompleted/{sortByUrgency:bool}")]
        public IActionResult GetNotCompletedRequests(bool sortByUrgency = false)
        {
            string query = SelectQuery + JoinItems + JoinReports + GetNotCompletedCondition;
            if (sortByUrgency)
                query += SortByUrgency;
            return Get<Request, SuppliedItem, Report, RequestsResponse>
                (query, new { }, Models.Request.MapQuery, Models.Request.MapModel);
        }
        

        [HttpDelete("{id:int}")]
        public IActionResult DeleteRequest(int id)
        {
            return Delete(DeleteQuery + GetOneCondition, new { Id = id });
        }

        [HttpPut]
        public IActionResult UpdateRequest(UpdateRequestRequest requestData)
        {
            return Models.Request.Validate(requestData).Match(result => Update(UpdateQuery, requestData), Problem);
        }

        [HttpPut("setReport")]
        public IActionResult SetReport(SetReportRequest requestData)
        {
            return Models.Request.ValidateReport(requestData.ReportId).Match(result => Update(SetReportQuery, requestData)
            , Problem);
        }
    }
}
