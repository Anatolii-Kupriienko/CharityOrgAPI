using API.Models;
using ErrorOr;

namespace API.Services.Projects
{
    public interface IProjectService
    {
        public ErrorOr<Created> CreateProject(Project requestData);
        public ErrorOr<Deleted> DeleteProject(int id);
        public ErrorOr<List<Project>> GetProject(int id);
        public ErrorOr<Updated> UpdateProject(Project requestData);
    }
}
