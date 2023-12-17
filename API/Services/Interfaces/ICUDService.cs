using ErrorOr;

namespace API.Services.Interfaces
{
    public interface ICUDService
    {
        ErrorOr<Created> Create<T>(string query, T requestData);
        ErrorOr<Updated> Update<T>(string query, T requestData);
        ErrorOr<Deleted> Delete(string query, int id);
    }
}
