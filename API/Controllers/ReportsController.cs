using API.Services.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ReportsController : ApiController
    {
        private readonly IReportsService _reportService;
        public ReportsController(IReportsService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        public IActionResult Create(UpsertReportsRequest requestData)
        {
            var createResult = _reportService.Create(requestData);
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
            var deleteResult =_reportService.Delete(id);
            return deleteResult.Match(result => Ok(), errors => Problem(errors));
        }

        [HttpPut]
        public IActionResult Update(UpsertReportsRequest requestData)
        {
            var updateResult = _reportService.Update(requestData);
            return updateResult.Match(result => Ok(), errors => Problem(errors));
        }
    }
}
