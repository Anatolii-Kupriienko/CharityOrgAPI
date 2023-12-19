using ErrorOr;

namespace API.Services.Interfaces
{
    public interface ICRUDService
    {
        ErrorOr<Created> Create(string query, object data);
        ErrorOr<List<T>> Get<T>(string query, int id);
        ErrorOr<List<T>> Get<T, V>(string query,  int id, Func<T, V, T> mapFunc);
        ErrorOr<List<T>> Get<T, V, U>(string query, int id, Func<T, V, U, T> mapFunc);
        ErrorOr<Updated> Update<T>(string query, T data);
        ErrorOr<Deleted> Delete(string query, int id);
        ErrorOr<int> GetByData<T>(string query, T data);
    }
}
