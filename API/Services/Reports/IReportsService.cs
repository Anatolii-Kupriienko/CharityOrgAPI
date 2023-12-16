using API.Models;
using ErrorOr;

namespace API.Services.Reports
{
    public interface IReportsService
    {
        ErrorOr<Created> Create(UpsertReportsRequest requestData);
        ErrorOr<Updated> Update(UpsertReportsRequest requestData);
        ErrorOr<Deleted> Delete(int id);
        ErrorOr<List<ReportsResponse>> Get(int id);
    }
}
