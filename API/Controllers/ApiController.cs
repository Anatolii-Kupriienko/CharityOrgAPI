﻿using API.Models;
using API.ServiceErrors;
using API.Services.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class ApiController(ICRUDService CRUDService) : ControllerBase
    {
        private readonly ICRUDService _CRUDService = CRUDService;

        protected IActionResult Create<T>(T insertData, string insertQuery, string getIdQuery, string actionName)
        {
            var createResult = _CRUDService.Create(insertQuery, insertData);
            if (createResult.IsError)
                return Problem(createResult.Errors);
            var id = _CRUDService.GetIdByData(getIdQuery, insertData).Value;
            return CreatedAtAction(actionName, new { Id = id }, new { });
        }

        protected IActionResult Create<T>(T insertData, string insertQuery, string actionName, int id)
        {
            return _CRUDService.Create(insertQuery, insertData).Match(result => CreatedAtAction(actionName, new { Id = id }, new { }), Problem);
        }

        protected IActionResult Get<T>(string query, object param)
        {
            return _CRUDService.Get<T>(query, param).Match(response => Ok(response), Problem);
        }

        protected IActionResult Get<T, V, X>(string query, object param, Func<T, V, T> mapQueryFunc, Func<List<T>, List<X>> mapModelFunc)
        {
            return _CRUDService.Get(query, param, mapQueryFunc).Match((result) => Ok(mapModelFunc(result)), Problem);
        }

        protected IActionResult Get<T, V, U, X>(string query, object param, Func<T, V, U, T> mapQueryFunc, Func<List<T>, List<X>> mapModelFunc)
        {
            return _CRUDService.Get(query, param, mapQueryFunc).Match(result => Ok(mapModelFunc(result)), Problem);
        }

        protected IActionResult Update<T>(string query, T queryParams)
        {
            return _CRUDService.Update(query, queryParams).Match(response => Ok(), Problem);
        }

        protected IActionResult Delete(string query, object param)
        {
            return _CRUDService.Delete(query, param).Match(response => Ok(), Problem);
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
