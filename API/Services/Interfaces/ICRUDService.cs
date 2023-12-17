using ErrorOr;

namespace API.Services.Interfaces
{
    public interface ICRUDService
    {
        ErrorOr<Created> Create(string query, object data);
        ErrorOr<List<T>> Get<T>(string query, int id);
        ErrorOr<List<T>> Get<T, V>(string query,  int id, Func<T, V, T> mapFunc);
        ErrorOr<Updated> Update<T>(string query, T data);
        ErrorOr<Deleted> Delete(string query, int id);
        ErrorOr<T> GetByData<T>(string query, T data);
    }
}
