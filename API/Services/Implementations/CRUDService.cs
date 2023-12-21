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

        public ErrorOr<Deleted> Delete(string query, int id)
        {
            int rowsDeleted = 0;
            try
            {
                rowsDeleted = DataAccess.UpdateData(query, new { Id = id });
            }
            catch
            {
                return Error.Unexpected();
            }
            if (rowsDeleted < 1)
                return Errors.General.NoResult;
            return Result.Deleted;
        }

        public ErrorOr<List<T>> Get<T>(string query, int id)
        {
            if (id < 0)
                return Errors.General.InvalidId;
            List<T> response;
            try
            {
                response = DataAccess.LoadData<T>(query, new { Id = id });
            }
            catch
            {
                return Error.Unexpected();
            }
            if (response.Count < 1)
                return Errors.General.NotFound;
            return response;
        }
        public ErrorOr<List<T>> Get<T, V>(string query, int id, Func<T, V, T> mapFunc)
        {
            List<T> response;
            try
            {
                response = DataAccess.LoadData(query, new { Id = id }, mapFunc);
            }
            catch
            {
                return Error.Unexpected();
            }
            if (response.Count < 1)
                return Errors.General.NotFound;

            return response;
        }

        public ErrorOr<List<T>> Get<T, V, U>(string query, int id, Func<T, V, U, T> mapFunc)
        {
            List<T> response;
            try
            {
                response = DataAccess.LoadData(query, new { Id = id }, mapFunc);
            }
            catch
            {
                return Error.Unexpected();
            }
            if (response.Count < 1)
                return Errors.General.NotFound;

            return response;
        }
        public ErrorOr<List<T>> Get<T, V, U>(string query, object data, Func<T, V, U, T> mapFunc)
        {
            List<T> response;
            try
            {
                response = DataAccess.LoadData(query, data, mapFunc);
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
            int rowsUpdated = 0;
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

        //change this and use for filtering
        public ErrorOr<int> GetByData<T>(string query, T data)
        {
            try
            {
                return DataAccess.LoadData<test>(query, data).Last().Id;
            }
            catch
            {
                return Errors.General.FailedToExecute;
            }
        }


    }
    public class test
    {
        public int Id;
        public test()
        {

        }
    }
}
