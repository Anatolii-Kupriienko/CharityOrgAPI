﻿using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PartnersController(ICRUDService cRUDService) : ApiController(cRUDService)
    {
        private readonly string SelectQuery = @"select * from partners";
        private readonly string SelectIdQuery = @"select id from partners where orgName = @OrgName and link = @Link";
        private readonly string GetOneCondition = @" where id = @Id";
        private readonly string InsertQuery = @"insert into partners(orgName, link)values(@OrgName, @Link)";
        private readonly string UpdateQuery = @"update partners set orgName = @OrgName, link = @Link where id = @Id";
        private readonly string DeleteQuery = @"delete from partners where id = @Id";

        [HttpPost]
        public IActionResult CreatePartner(CreatePartnerRecord requestData)
        {
            return Partner.ValidateStrings(requestData.OrgName, requestData.Link)
            .Match(result => Create(requestData, InsertQuery, SelectIdQuery, nameof(GetPartner)), 
            Problem);
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetPartner(int id = 0)
        {
            string query = SelectQuery;
            if (id != 0)
                query += GetOneCondition;

            return Get<UpdatePartnerRequestResponse>(query, new {Id = id});
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletePartner(int id)
        {
            return Delete(DeleteQuery, new { Id = id });
        }

        [HttpPut]
        public IActionResult UpdatePartner(UpdatePartnerRequestResponse requestData)
        {
            return Partner.ValidateUpdate(requestData)
                .Match(result => Update(UpdateQuery, requestData), Problem);
        }
    }
}
