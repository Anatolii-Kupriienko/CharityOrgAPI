using API.Models;
using API.ServiceErrors;
using API.Services.SupportDirections;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SupportDirectionsController : ApiController
    {
        private readonly ISupportDirectionService _supportDirectionService;

        public SupportDirectionsController()
        {
            _supportDirectionService = new SupportDirectionService();
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetSupportDirection(int id = 0)
        {
            var responseResult = _supportDirectionService.GetSupportDirection(id);
            return responseResult.Match(response => Ok(response), errors => Problem(errors));
        }

        [HttpPost]
        public IActionResult CreateSupportDirection(UpsertSupportDirectionRequest requestData)
        {
            var mapResult = MapRequestToModel(requestData);
            if (mapResult.IsError)
                return Problem(mapResult.Errors);
            _supportDirectionService.CreateSupportDirection(mapResult.Value);
            var id = SupportDirectionService.GetSupportDirectionId(mapResult.Value);
            return CreatedAtAction(nameof(GetSupportDirection), id, new {});
        }

        [HttpPut]
        public IActionResult UpdateSupportDirection(UpsertSupportDirectionRequest requestData)
        {
            if (requestData.Id is null or < 1)
                return Problem(new List<Error> { Errors.SupportDirection.InvalidId });

            var mapResult = MapRequestToModel(requestData);

            if (mapResult.IsError)
                return Problem(mapResult.Errors);

            var updateResult = _supportDirectionService.UpdateSupportDirection(mapResult.Value);
            return updateResult.Match(response => NoContent(), errors => Problem(errors));
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteSupportDirection(int id)
        {
            var deleteResult = _supportDirectionService.DeleteSupportDirection(id);
            return deleteResult.Match(response => NoContent(), errors => Problem(errors));
        }
        private ErrorOr<SupportDirection> MapRequestToModel(UpsertSupportDirectionRequest requestData)
        {
            var requestToDataResult = SupportDirection.Create(requestData.Name, requestData.Description, requestData.About, requestData.Id);
            if (requestToDataResult.IsError)
                return requestToDataResult.Errors;
            return requestToDataResult.Value;
        }
    }
}
