using API.Models;
using API.ServiceErrors;
using API.Services.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class ApiController : ControllerBase
    {
        private readonly ICRUDService _CRUDService;
        public ApiController(ICRUDService CRUDService)
        {
            _CRUDService = CRUDService;
        }

        protected IActionResult Create<T>(T insertData, string insertQuery, string getIdQuery, string actionName)
        {
            var createResult = _CRUDService.Create(insertQuery, insertData);
            if (createResult.IsError)
                return Problem(createResult.Errors);
            var id = _CRUDService.GetByData(getIdQuery, insertData).Value;
            return CreatedAtAction(actionName, new { Id = id }, new { });
        }
        public IActionResult Get<T>(string query, int id)
        {
            var responseResult = _CRUDService.Get<T>(query, id);
            return responseResult.Match(response => Ok(response), errors => Problem(errors));
        }
        
        public IActionResult Get<T, V, X>(string query, int id, Func<T, V, T> mapQueryFunc, Func<List<T>, List<X>> mapModelFunc)
        {
            var getResult = _CRUDService.Get(query, id, mapQueryFunc);
            return getResult.Match((result) => Ok(mapModelFunc(result)), errors => Problem(errors));
        }

        public IActionResult Update<T>(string query, T queryParams)
        {
            var updateResult = _CRUDService.Update(query, queryParams);
            return updateResult.Match(response => NoContent(), errors => Problem(errors));
        }

        public IActionResult Delete(string query, int id)
        {
            var deleteResult = _CRUDService.Delete(query, id);
            return deleteResult.Match(response => NoContent(), errors => Problem(errors));
        }
        protected IActionResult Problem(List<Error> errors)
        {
            if (errors.All(e => e.Type == ErrorType.Validation))
            {
                var modelStateDictionary = new ModelStateDictionary();

                foreach (var error in errors)
                {
                    modelStateDictionary.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem(modelStateDictionary);
            }

            if (errors.Any(e => e.Type == ErrorType.Unexpected))
            {
                return Problem();
            }

            var firstError = errors[0];

            var statusCode = firstError.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Failure => StatusCodes.Status417ExpectationFailed,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(statusCode: statusCode, title: firstError.Description);
        }
    }
}
