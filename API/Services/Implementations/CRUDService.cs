using API.Models;
using API.ServiceErrors;
using API.Services.Interfaces;
using ErrorOr;

namespace API.Services.Implementations
{
    public class SimpleCRUDService : ICRUDService
    {
        public ErrorOr<Created> Create(string query, object data)
        {
            try
            {
                DataAccess.InsertData(query, data);
                return Result.Created;
            }
            catch
            {
                return Errors.General.FailedToExecute;
            }
        }

        public ErrorOr<Deleted> Delete(string query, object param)
        {
            int rowsDeleted;
            try
            {
                rowsDeleted = DataAccess.UpdateData(query, param);
            }
            catch
            {
                return Error.Unexpected();
            }
            if (rowsDeleted < 1)
                return Errors.General.NoResult;
            return Result.Deleted;
        }

        public ErrorOr<List<T>> Get<T>(string query, object param)
        {
            List<T> response;
            try
            {
                response = DataAccess.LoadData<T>(query, param);
            }
            catch
            {
                return Error.Unexpected();
            }
            if (response.Count < 1)
                return Errors.General.NotFound;
            return response;
        }
        public ErrorOr<List<T>> Get<T, V>(string query, object param, Func<T, V, T> mapFunc)
        {
            List<T> response;
            try
            {
                response = DataAccess.LoadData(query, param, mapFunc);
            }
            catch
            {
                return Error.Unexpected();
            }
            if (response.Count < 1)
                return Errors.General.NotFound;

            return response;
        }

        public ErrorOr<List<T>> Get<T, V, U>(string query, object param, Func<T, V, U, T> mapFunc)
        {
            List<T> response;
            try
            {
                response = DataAccess.LoadData(query, param, mapFunc);
            }
            catch
            {
                return Error.Unexpected();
            }
            if (response.Count < 1)
                return Errors.General.NotFound;

            return response;
        }

        public ErrorOr<Updated> Update<T>(string query, T data)
        {
            int rowsUpdated;
            try
            {
                rowsUpdated = DataAccess.UpdateData(query, data);
            }
            catch
            {
                return Errors.General.FailedToExecute;
            }
            if (rowsUpdated < 1)
                return Errors.General.NoResult;
            return Result.Updated;
        }

        public ErrorOr<int> GetIdByData<T>(string query, T data)
        {
            try
            {
                return DataAccess.LoadData<SupportGetClass>(query, data).Last().Id;
            }
            catch
            {
                return Errors.General.FailedToExecute;
            }
        }
    }
    public class SupportGetClass(int Id)
    {
        public int Id { get; } = Id;
    }
}
