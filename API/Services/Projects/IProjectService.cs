using API.Models;
using ErrorOr;

namespace API.Services.Projects
{
    public interface IProjectService
    {
        ErrorOr<Created> CreateProject(Project requestData);
        ErrorOr<Updated> UpdateProject(Project requestData);
        ErrorOr<Deleted> DeleteProject(int id);
        ErrorOr<List<Project>> GetProject(int id);
    }
}
