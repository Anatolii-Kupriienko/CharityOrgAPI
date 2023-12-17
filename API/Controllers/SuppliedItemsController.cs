using API.Models;
using API.ServiceErrors;
using API.Services.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace API.Controllers
{
    public class SuppliedItemsController(ICRUDService _CRUDService) : ApiController
    {
        private readonly ICRUDService _CRUDService = _CRUDService;
        private readonly string InsertQuery = @"insert into suppliedItems(name, amountSupplied, generalName)values(@Name, @AmountSupplied, @GeneralName)";
        private readonly string SelectQuery = @"select * from suppliedItems";
        private readonly string GetOneCondition = @" where id = @Id";
        private readonly string GetIdCondition = @" where name = @Name and generalName = @GeneralName";
        private readonly string UpdateQuery = @"update suppliedItems set name = @Name, amountSupplied = @AmountSupplied, generalName = @GeneralName where id = @Id";
        private readonly string DeleteQuery = @"delete from suppliedItems where id = @Id";

        [HttpPost]
        public IActionResult CreateItem(ItemRecord requestData)
        {
            var validationResult = SuppliedItem.Create(requestData.Id, requestData.Name, requestData.AmountSupplied, requestData.GeneralName);
            if (validationResult.IsError)
                return Problem(validationResult.Errors);
            var createResult = _CRUDService.Create(InsertQuery, validationResult.Value);
            if (createResult.IsError)
                return Problem(createResult.Errors);
            var id = _CRUDService.GetByData(SelectQuery + GetIdCondition, validationResult.Value).Value.Id;
            return CreatedAtAction(nameof(GetItem), new { Id = id }, new { });
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetItem(int id = 0)
        {
            string query = SelectQuery;
            if (id != 0)
                query += GetOneCondition;
            var responseResult = _CRUDService.Get<SuppliedItem>(query, id);
            return responseResult.Match(response => Ok(response), errors => Problem(errors));
        }

        [HttpPut]
        public IActionResult UpdateItem(ItemRecord requestData)
        {
            if (requestData.Id is null or < 1)
                return Problem(new List<Error> { Errors.General.InvalidId });

            var validationResult = SuppliedItem.Create(requestData.Id, requestData.Name, requestData.AmountSupplied, requestData.GeneralName);

            if (validationResult.IsError)
                return Problem(validationResult.Errors);

            var updateResult = _CRUDService.Update(UpdateQuery, validationResult.Value);
            return updateResult.Match(response => NoContent(), errors => Problem(errors));
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteItem(int id)
        {
            var deleteResult = _CRUDService.Delete(DeleteQuery, id);
            return deleteResult.Match(response => NoContent(), errors => Problem(errors));
        }
    }
}
