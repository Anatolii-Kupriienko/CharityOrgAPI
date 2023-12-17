using ErrorOr;

namespace API.Services.Interfaces
{
    public interface ISimpleCRUDService
    {
        ErrorOr<Created> Create(string query, object data);
        ErrorOr<List<T>> Get<T>(string query, int id);
        ErrorOr<Updated> Update<T>(string query, T data);
        ErrorOr<Deleted> Delete(string query, int id);
        ErrorOr<T> GetByData<T>(string query, T data);
    }
}
