using API.Models;
using API.ServiceErrors;
using API.Services.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace API.Controllers
{
    public class SuppliedItemsController : ApiController
    {
        private readonly ICRUDService _CRUDService;
        private readonly string InsertQuery = @"insert into suppliedItems(name, amountSupplied, generalName)values(@Name, @AmountSupplied, @GeneralName)";
        private readonly string SelectQuery = @"select * from suppliedItems";
        private readonly string GetOneCondition = @" where id = @Id";
        private readonly string GetIdCondition = @" where name = @Name and generalName = @GeneralName";
        private readonly string UpdateQuery = @"update suppliedItems set name = @Name, amountSupplied = @AmountSupplied, generalName = @GeneralName where id = @Id";
        private readonly string DeleteQuery = @"delete from suppliedItems where id = @Id";

        public SuppliedItemsController(ICRUDService CRUDService) : base(CRUDService)
        {
            _CRUDService = CRUDService;
        }

        [HttpPost]
        public IActionResult CreateItem(ItemRecord requestData)
        {
            var validationResult = SuppliedItem.Create(requestData.Id, requestData.Name, requestData.AmountSupplied, requestData.GeneralName);
            if (validationResult.IsError)
                return Problem(validationResult.Errors);
            return Create(validationResult.Value, InsertQuery, SelectQuery + GetIdCondition, nameof(GetItem));
            
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetItem(int id = 0)
        {
            string query = SelectQuery;
            if (id != 0)
                query += GetOneCondition;
            return Get<SuppliedItem>(query, id);
        }

        [HttpPut]
        public IActionResult UpdateItem(ItemRecord requestData)
        {
            if (requestData.Id is null or < 1)
                return Problem(new List<Error> { Errors.General.InvalidId });

            var validationResult = SuppliedItem.Create(requestData.Id, requestData.Name, requestData.AmountSupplied, requestData.GeneralName);

            if (validationResult.IsError)
                return Problem(validationResult.Errors);

            return Update(UpdateQuery, validationResult.Value);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteItem(int id)
        {
            return Delete(DeleteQuery, id);
        }
    }
}
