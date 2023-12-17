using API.Models;
using API.Services.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class ReportsController(ICRUDService _CRUDService) : ApiController 
    {
        private readonly string InsertQuery = @"insert into reports(dateFulfilled, buyingRecordsLink, recieverReportLink, projectId)values(@DateFulfilled, @BuyingRecordsLink, @RecieverReportLink, @ProjectId)";
        private readonly string DeleteQuery = @"delete from reports where id = @Id";
        private readonly string UpdateQuery = @"update reports set dateFulfilled = @DateFulfilled, buyingRecordsLink = @BuyingRecordsLink, recieverReportLink = @RecieverReportLink, projectId = @ProjectId where id = @Id";
        private readonly string SelectQuery = @"select * from reports";
        private readonly string GetIdCondition = @" where dateFulfilled = @DateFulfilled and buyingRecordsLink = @BuyingRecordsLink";
        private readonly string GetOneCondition = @" where reports.id = @Id";
        private readonly string Join = @" left join projects on projects.id = projectId";
        private readonly ICRUDService _CRUDService = _CRUDService;

        [HttpPost]
        public IActionResult Create(UpsertReportsRequest requestData)
        {

            var createResult = _CRUDService.Create(InsertQuery, requestData);
            if (createResult.IsError)
                return Problem(createResult.Errors);
            var id = _CRUDService.GetByData(SelectQuery + GetIdCondition, requestData).Value.Id;
            return CreatedAtAction(nameof(Get), new { Id = id }, new {});
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult Get(int id = 0)
        {
            string query = SelectQuery + Join;
            if (id != 0)
                query += GetOneCondition;
            var getResult = _CRUDService.Get<Report, Project>(query, id, Report.MapQuery);
            return getResult.Match((result)=>Ok(Report.MapModel(result)), errors => Problem(errors));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var deleteResult = _CRUDService.Delete(DeleteQuery, id);
            return deleteResult.Match(result => Ok(), errors => Problem(errors));
        }

        [HttpPut]
        public IActionResult Update(UpsertReportsRequest requestData)
        {
            var updateResult = _CRUDService.Update(UpdateQuery, requestData);
            return updateResult.Match(result => Ok(), errors => Problem(errors));
        }
    }
}
