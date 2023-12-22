using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProjectPartnersController(ICRUDService cRUDService) : ApiController(cRUDService)
    {
        private readonly string SelectQuery = @"select * from projectPartners";
        private readonly string JoinPartners = @" left join partners on partners.id = partnerId";
        private readonly string JoinProjects = @" left join projects on projects.id = projectId";
        private readonly string GetOneCondition = @" where projectPartners.id = @Id";
        private readonly string GetOneByPartnerCondition = @" where partnerId = @Id";
        private readonly string GetOneByProjectCondition = @" where projectId = @Id";
        private readonly string SelectIdQuery = @"select id from projectPartners where partnerId = @PartnerId and projectId = @ProjectId";
        private readonly string InsertQuery = @"insert into projectPartners(partnerId, projectId)values(@PartnerId, @ProjectId)";
        private readonly string UpdateQuery = @"update projectPartners set partnerId = @ProjectId, projectId = @ProjectId where id = @Id";
        private readonly string DeleteQuery = @"delete from projectPartners";

        [HttpPost]
        public IActionResult CreateProjectPartner(CreateProjectPartnerRequest requestData)
        {
            return ProjectPartner.ValidateRequest(null, requestData.PartnerId, requestData.ProjectId).Match(
                result => Create(requestData, InsertQuery, SelectIdQuery, nameof(GetProjectPartner))
                , Problem);
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetProjectPartner(int id = 0)
        {
            string query = SelectQuery + JoinPartners + JoinProjects;
            if (id != 0)
                query += GetOneCondition;
            return Get<ProjectPartner, Partner, Project, ProjectPartnerResponse>
                (query, new {Id = id}, ProjectPartner.MapQuery, ProjectPartner.MapModel);
        }


        [HttpGet("partner")]
        [HttpGet("partner/{id:int}")]
        public IActionResult GetProjectsForPartners(int id = 0)
        {
            string query = SelectQuery + JoinPartners + JoinProjects;
            if (id != 0)
                query += GetOneByPartnerCondition;
            return Get<ProjectPartner, Partner, Project, FilteredByPartnerProjectPartnerResponse>
                (query, new { Id = id }, ProjectPartner.MapQuery, ProjectPartner.MapFileteredPartnerModel);
        }

        [HttpGet("project")]
        [HttpGet("project/{id:int}")]
        public IActionResult GetPartnersForProjects(int id = 0)
        {
            string query = SelectQuery + JoinPartners + JoinProjects;
            if (id != 0)
                query += GetOneByProjectCondition;
            return Get<ProjectPartner, Partner, Project, FilteredByProjectPartnerResponse>
                (query, new { Id = id }, ProjectPartner.MapQuery, ProjectPartner.MapFilteredByProjectModel);
        }

        [HttpPut]
        public IActionResult UpdatePartnerProject(UpdateProjectPartnerRequest requestData)
        {
            return ProjectPartner.ValidateRequest(requestData.Id, requestData.PartnerId, requestData.ProjectId)
                .Match(result => Update(UpdateQuery, requestData), Problem);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletePartnerProject(int id)
        {
            return Delete(DeleteQuery + GetOneCondition, new { Id = id });
        }

        [HttpDelete("partners/{id:int}")]
        public IActionResult DeletePartnerProjectByPartner(int id)
        {
            return Delete(DeleteQuery + GetOneByPartnerCondition, new { Id = id });
        }

        [HttpDelete("projects/{id:int}")]
        public IActionResult DeletePartnerProjectByProject(int id)
        {
            return Delete(DeleteQuery + GetOneByProjectCondition, new { Id = id });
        }
    }
}
