using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProjectItemsController : ApiController
    {
        public ProjectItemsController(ICRUDService cRUDService): base(cRUDService) { }
        private readonly string SelectQuery = @"select * from projectItems";
        private readonly string JoinItems = @" left join suppliedItems on suppliedItems.id = itemId";
        private readonly string JoinProjects = @" left join projects on projects.id = projectId";
        private readonly string GetOneItemCondition = @" where itemId = @Id";
        private readonly string GetOneProjectCondition = @" where projectId = @Id";
        private readonly string InsertQuery = @"insert into projectItems(itemId, projectId)values(@ItemId, @ProjectId)";
        private readonly string UpdateQuery = @"update projectItems set projectId = @ProjectId where itemId = @ItemId";
        private readonly string DeleteQuery = @"delete from projectItems where itemId = @Id";

        [HttpPost]
        public IActionResult CreateProjectItem(UpsertProjectItemsReuqest requestData)
        {
            return ProjectItem.ValidateRequest(requestData).
                Match(result => Create(requestData, InsertQuery, nameof(GetProjectItem), requestData.ItemId),
                Problem);
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetProjectItem(int id = 0)
        {
            string query = SelectQuery + JoinItems + JoinProjects;
            if (id != 0)
                query += GetOneItemCondition;
            return Get<ProjectItem, SuppliedItem, Project, ProjectItemsResponse>
                (query, new {Id = id}, ProjectItem.MapQuery, ProjectItem.MapModel);
        }

        [HttpGet("project")]
        [HttpGet("project/{id:int}")]
        public IActionResult GetAllItemsForProject(int id = 0)
        {
            string query = SelectQuery + JoinItems + JoinProjects;
            if (id != 0)
                query += GetOneProjectCondition;
            return Get<ProjectItem, SuppliedItem, Project, FilteredProjectItemResponse>
                (query, new {Id = id}, ProjectItem.MapQuery, ProjectItem.MapFilteredModel);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteProjectItem(int id)
        {
            return Delete(DeleteQuery, new { Id = id });
        }

        [HttpPut]
        public IActionResult UpdateProjectItem(UpsertProjectItemsReuqest requestData)
        {
            return ProjectItem.ValidateRequest(requestData).Match(result => Update(UpdateQuery, requestData), Problem);
        }
    }
}
