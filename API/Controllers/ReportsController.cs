using API.Models;
using API.Services.Interfaces;
using API.Services.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class ReportsController(IReportsService _reportService, ICUDService _CUDService) : ApiController 
    {
        private readonly string InsertQuery = @"insert into reports(dateFulfilled, buyingRecordsLink, recieverReportLink, projectId)values(@DateFulfilled, @BuyingRecordsLink, @RecieverReportLink, @ProjectId)";
        private readonly string DeleteQuery = @"delete from reports where id = @Id";
        private readonly string UpdateQuery = @"update reports set dateFulfilled = @DateFulfilled, buyingRecordsLink = @BuyingRecordsLink, recieverReportLink = @RecieverReportLink, projectId = @ProjectId where id = @Id";
        private readonly IReportsService _reportService = _reportService;
        private readonly ICUDService _CUDService = _CUDService;

        [HttpPost]
        public IActionResult Create(UpsertReportsRequest requestData)
        {

            var createResult = _CUDService.Create(InsertQuery, requestData);
            if (createResult.IsError)
                return Problem(createResult.Errors);
            var id = ReportsService.GetReportId(requestData);
            return CreatedAtAction(nameof(Get), new { Id = id }, new {});
        }


        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult Get(int id = 0)
        {
            var getResult = _reportService.Get(id);
            return getResult.Match((result)=>Ok(result), errors => Problem(errors));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var deleteResult =_CUDService.Delete(DeleteQuery, id);
            return deleteResult.Match(result => Ok(), errors => Problem(errors));
        }

        [HttpPut]
        public IActionResult Update(UpsertReportsRequest requestData)
        {
            var updateResult = _CUDService.Update(UpdateQuery, requestData);
            return updateResult.Match(result => Ok(), errors => Problem(errors));
        }
    }
}
