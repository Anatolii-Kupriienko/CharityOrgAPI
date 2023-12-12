using API.Models;
using API.ServiceErrors;
using API.Services.Projects;
using API.Services.SupportDirections;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProjectsController : ApiController
    {
        private readonly IProjectService _projectService;
        public ProjectsController()
        {
            _projectService = new ProjectService();
        }

        [HttpGet]
        [HttpGet("{id:int}")]
        public IActionResult GetProject(int id = 0)
        {
            var responseResult = _projectService.GetProject(id);
            return responseResult.Match(response => Ok(response), errors => Problem(errors));
        }

        [HttpPost]
        public IActionResult CreateProject(CreateProjectRequest requestData)
        {
            var mapResult = Project.Create(null, requestData.Name, requestData.TotalPrice, requestData.StartDate, null, requestData.Link, requestData.IsWithPartners, requestData.IsMilitary, null);
            if (mapResult.IsError)
                return Problem(mapResult.Errors);
            _projectService.CreateProject(mapResult.Value);
            var id = ProjectService.GetProjectId(mapResult.Value);
            return CreatedAtAction(nameof(GetProject), id, new { });
        }

        [HttpPut]
        public IActionResult UpdateProject(UpdateProjectRequest requestData)
        {
            if (requestData.Id < 1)
                return Problem(new List<Error> { Errors.SupportDirection.InvalidId });

            var mapResult = Project.Create(requestData.Id, requestData.Name, requestData.TotalPrice, requestData.StartDate, requestData.FinishDate, requestData.Link, requestData.IsWithPartners, requestData.IsMilitary, requestData.TotalFundsCollected);

            if (mapResult.IsError)
                return Problem(mapResult.Errors);

            var updateResult = _projectService.UpdateProject(mapResult.Value);
            return updateResult.Match(response => NoContent(), errors => Problem(errors));
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteProject(int id)
        {
            var deleteResult = _projectService.DeleteProject(id);
            return deleteResult.Match(response => NoContent(), errors => Problem(errors));
        }
    }
}
