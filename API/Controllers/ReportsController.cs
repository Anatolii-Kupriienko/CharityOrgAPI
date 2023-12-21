using API.Models;
using API.Services.Interfaces;
using ErrorOr;
using Kursova.Contracts.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class ReportsController(ICRUDService CRUDService) : ApiController(CRUDService) 
    {
        private readonly string InsertQuery = @"insert into reports(dateFulfilled, buyingRecordsLink, recieverReportLink, projectId)values(@DateFulfilled, @BuyingRecordsLink, @RecieverReportLink, @ProjectId)";
        private readonly string DeleteQuery = @"delete from reports where id = @Id";
        private readonly string UpdateQuery = @"update reports set dateFulfilled = @DateFulfilled, buyingRecordsLink = @BuyingRecordsLink, recieverReportLink = @RecieverReportLink, projectId = @ProjectId where id = @Id";
        private readonly string SelectQuery = @"select * from reports";
        private readonly string GetIdCondition = @" where dateFulfilled = @DateFulfilled and buyingRecordsLink = @BuyingRecordsLink";
        private readonly string GetOneCondition = @" where reports.id = @Id";
        private readonly string GetForProjectCondition = @" where projectId = @Id";
        private readonly string GetBetweenDatesCondition = @" where dateFulfilled between @StartDate and @EndDate";
        private readonly string OrderCondition = @" order by dateFulfilled ";
        private readonly string Join = @" left join projects on projects.id = projectId";

        [HttpPost]
        public IActionResult Create(UpsertReportsRequest requestData)
        {
            var validationResult = Report.ValidateReport(requestData);
            if (validationResult.IsError)
                return Problem(validationResult.Errors);

            return Create(requestData, InsertQuery, SelectQuery + GetIdCondition, nameof(GetReport));
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetReport(int id = 0)
        {
            string query = SelectQuery + Join;
            if (id != 0)
                query += GetOneCondition;

            return Get<Report, Project, ReportsResponse>(query, new {Id = id}, Report.MapQuery, Report.MapModel);
        }

        [HttpGet("byDate")]
        [HttpGet("byDate/{desc:bool}")]
        public IActionResult GetReportsSorted(bool desc = false)
        {
            string query = SelectQuery + Join + OrderCondition;
            if (desc)
                query += "desc";
            return Get<Report, Project, ReportsResponse>(query, new { }, Report.MapQuery, Report.MapModel);
        }

        [HttpGet("{startDate:datetime}/{endDate:datetime}")]
        public IActionResult GetReportsBetweenDates(DateTime startDate, DateTime endDate)
        {
            string query = SelectQuery + Join + GetBetweenDatesCondition;
            return Get<Report, Project, ReportsResponse>
                (query, new { StartDate = startDate, EndDate = endDate }, Report.MapQuery, Report.MapModel);
        }

        [HttpGet("{startDate:datetime}/{endDate:datetime}/byDate/{desc:bool}")]
        [HttpGet("{startDate:datetime}/{endDate:datetime}/byDate")]
        public IActionResult GetReportsBetweenDatesSorted(DateTime startDate, DateTime endDate, bool desc = false)
        {
            string query = SelectQuery + Join + GetBetweenDatesCondition + OrderCondition;
            if (desc)
                query += "desc";
            return Get<Report, Project, ReportsResponse>
                (query, new { StartDate = startDate, EndDate = endDate }, Report.MapQuery, Report.MapModel);
        }

        [HttpGet("projects")]
        [HttpGet("projects/{id:int}")]
        public IActionResult GetReportsByProjects(int id = 0)
        {
            string query = SelectQuery + Join;
            if (id != 0)
                query += GetForProjectCondition;
            return Get<Report, Project, ReportsByProjectResponse>
                (query, new { Id = id }, Report.MapQuery, Report.MapFilteredModel);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteReport(int id)
        {
            return Delete(DeleteQuery, new { Id = id });
        }

        [HttpPut]
        public IActionResult UpdateReport(UpsertReportsRequest requestData)
        {
            var validationResult = Report.ValidateReport(requestData);
            if (validationResult.IsError)
                return Problem(validationResult.Errors);

            return Update(UpdateQuery, requestData);
        }
    }
}
