using ErrorOr;

namespace API.Services.Interfaces
{
    public interface ICRUDService
    {
        ErrorOr<Created> Create(string query, object data);
        ErrorOr<List<T>> Get<T>(string query, object param);
        ErrorOr<List<T>> Get<T, V>(string query,  object param, Func<T, V, T> mapFunc);
        ErrorOr<List<T>> Get<T, V, U>(string query, object param, Func<T, V, U, T> mapFunc);
        ErrorOr<Updated> Update<T>(string query, T data);
        ErrorOr<Deleted> Delete(string query, object param);
        ErrorOr<int> GetIdByData<T>(string query, T data);
    }
}
