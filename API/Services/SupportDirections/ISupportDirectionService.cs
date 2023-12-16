using API.Models;
using API.Services.Employees;
using ErrorOr;

namespace API.Services.SupportDirections
{
    public interface ISupportDirectionService
    {
        ErrorOr<Created> CreateSupportDirection(SupportDirection data);
        ErrorOr<Updated> UpdateSupportDirection(SupportDirection data);
        ErrorOr<Deleted> DeleteSupportDirection(int id);
        ErrorOr<List<SupportDirection>> GetSupportDirection(int id);
    }
}
