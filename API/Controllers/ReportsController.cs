using API.Models;
using API.Services.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class ReportsController : ApiController 
    {
        public ReportsController(ICRUDService CRUDService) : base(CRUDService)
        {
            _CRUDService = CRUDService;
        }
        private readonly ICRUDService _CRUDService;
        private readonly string InsertQuery = @"insert into reports(dateFulfilled, buyingRecordsLink, recieverReportLink, projectId)values(@DateFulfilled, @BuyingRecordsLink, @RecieverReportLink, @ProjectId)";
        private readonly string DeleteQuery = @"delete from reports where id = @Id";
        private readonly string UpdateQuery = @"update reports set dateFulfilled = @DateFulfilled, buyingRecordsLink = @BuyingRecordsLink, recieverReportLink = @RecieverReportLink, projectId = @ProjectId where id = @Id";
        private readonly string SelectQuery = @"select * from reports";
        private readonly string GetIdCondition = @" where dateFulfilled = @DateFulfilled and buyingRecordsLink = @BuyingRecordsLink";
        private readonly string GetOneCondition = @" where reports.id = @Id";
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

            return Get<Report, Project, ReportsResponse>(query, id, Report.MapQuery, Report.MapModel);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteReport(int id)
        {
            return Delete(DeleteQuery, id);
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
