using API.Models;
using API.Services.Employees;
using ErrorOr;

namespace API.Services.SupportDirections
{
    public interface ISupportDirectionService
    {
        public ErrorOr<List<SupportDirection>> GetSupportDirection(int id);
        public ErrorOr<Created> CreateSupportDirection(SupportDirection data);
        public ErrorOr<Updated> UpdateSupportDirection(SupportDirection data);
        public ErrorOr<Deleted> DeleteSupportDirection(int id);
    }
}
