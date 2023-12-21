using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RequestsController : ApiController
    {
        public RequestsController(ICRUDService cRUDService) : base(cRUDService) { }
        private readonly string SelectQuery = @"select * from requests";
        private readonly string GetOneCondition = @" where requests.id = @Id";
        private readonly string GetIdCondition = @" where requester = @Requester and itemId = @ItemId and amount = @Amount and dateRecieved = @DateRecieved and urgency = @Urgency";
        private readonly string OrderByDateCondition = @" order by dateRecieved ";
        private readonly string GetByRequesterCondition = @" where requester = @Requester";
        private readonly string JoinItems = @" left join suppliedItems on suppliedItems.id = itemId";
        private readonly string JoinReports = @" left join reports on reports.id = reportId";
        private readonly string InsertQuery = @"insert into requests(itemId, amount, dateRecieved, urgency, requester)values(@ItemId, @Amount, @DateRecieved, @Urgency, @Requester)";
        private readonly string DeleteQuery = @"delete from requests";
        private readonly string UpdateQuery = @"update requests set itemId = @ItemId, amount = @Amount, dateRecieved = @DateRecieved, urgency = @Urgency, requester = @Requester, reportId = @ReportId where id = @Id";
        private readonly string SetReportQuery = @"update requests set reportId = @ReportId where id = @Id";


        [HttpPost]
        public IActionResult CreateRequest(CreateRequestRequest requestData)
        {
            return Models.Request.Create(requestData).
                Match(result => Create(result, InsertQuery, SelectQuery + GetIdCondition, nameof(GetRequest))
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
                (query, id, Models.Request.MapQuery, Models.Request.MapModel);
        }

        [HttpGet("orderByDate")]
        [HttpGet("orderByDate/{desc:bool}")]
        public IActionResult GetRequestsOrderedByDate(bool desc = false)
        {
            string query = SelectQuery + JoinItems + JoinReports + OrderByDateCondition;
            if (desc)
                query += "desc";
            return Get<Request, SuppliedItem, Report, RequestsResponse>
                (query, 0, Models.Request.MapQuery, Models.Request.MapModel);
        }


        [HttpGet("requester")]
        [HttpGet("requester/{requesterName}")]
        public IActionResult GetRequestsForRequester(string requesterName = "")
        {
            string query = SelectQuery + JoinItems + JoinReports;
            if (requesterName.Length > 0)
                query += GetByRequesterCondition;
            return Get<Request, SuppliedItem, Report, AllRequestsByRequesterResponse>
                (query, new { Requester = requesterName }, Models.Request.MapQuery, Models.Request.ModelFilteredModel);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteRequest(int id)
        {
            return Delete(DeleteQuery + GetOneCondition, id);
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
