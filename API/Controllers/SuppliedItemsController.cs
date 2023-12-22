using API.Models;
using API.ServiceErrors;
using API.Services.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace API.Controllers
{
    public class SuppliedItemsController(ICRUDService CRUDService) : ApiController(CRUDService)
    {
        private readonly string InsertQuery = @"insert into suppliedItems(name, amountSupplied, generalName)values(@Name, @AmountSupplied, @GeneralName)";
        private readonly string SelectQuery = @"select * from suppliedItems";
        private readonly string GetOneCondition = @" where id = @Id";
        private readonly string SelectIdQuery = @"select id from suppliedItems where name = @Name and generalName = @GeneralName";
        private readonly string UpdateQuery = @"update suppliedItems set name = @Name, amountSupplied = @AmountSupplied, generalName = @GeneralName where id = @Id";
        private readonly string DeleteQuery = @"delete from suppliedItems where id = @Id";
        private readonly string OrderCondition = @" order by amountSupplied ";
        private readonly string GetByNameCondition = @" where generalName like @Name or name like @Name";


        [HttpPost]
        public IActionResult CreateItem(ItemRecord requestData)
        {
            var validationResult = SuppliedItem.Create(requestData.Id, requestData.Name, requestData.AmountSupplied, requestData.GeneralName);
            if (validationResult.IsError)
                return Problem(validationResult.Errors);
            return Create(validationResult.Value, InsertQuery, SelectIdQuery, nameof(GetItem));
            
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetItem(int id = 0)
        {
            string query = SelectQuery;
            if (id != 0)
                query += GetOneCondition;
            return Get<SuppliedItem>(query, new {Id = id});
        }

        [HttpGet("name/{name}")]
        public IActionResult GetItemsByName(string name)
        {
            string query = SelectQuery + GetByNameCondition;
            return Get<SuppliedItem>(query, new { Name = $"%{name}%" });
        }

        [HttpGet("name/sorted/{desc:bool}")]
        [HttpGet("name/sorted/{desc:bool}/{name}")]
        public IActionResult GetItemsByNameSorted(bool desc = false, string? name = null)
        {
            string query = SelectQuery;
            if (name != null)
                query += GetByNameCondition;
            query += OrderCondition;
            if (desc)
                query += "desc";
            return Get<SuppliedItem>(query, new { Name = $"%{name}%" });
        }

        [HttpPut]
        public IActionResult UpdateItem(ItemRecord requestData)
        {
            if (requestData.Id is null or < 1)
                return Problem([Errors.General.InvalidId]);

            var validationResult = SuppliedItem.Create(requestData.Id, requestData.Name, requestData.AmountSupplied, requestData.GeneralName);

            if (validationResult.IsError)
                return Problem(validationResult.Errors);

            return Update(UpdateQuery, validationResult.Value);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteItem(int id)
        {
            return Delete(DeleteQuery, new { Id = id });
        }
    }
}
