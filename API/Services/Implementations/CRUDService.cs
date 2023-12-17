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
            int rowsDeleted = DataAccess.UpdateData(query, new { Id = id });
            if (rowsDeleted < 1)
                return Errors.General.NoResult;
            return Result.Deleted;
        }

        public ErrorOr<List<T>> Get<T>(string query, int id)
        {
            if (id < 0)
                return Errors.General.InvalidId;
            var response = DataAccess.LoadData<T>(query, new { Id = id });
            if (response.Count < 1)
                return Errors.General.NotFound;
            return response;
        }
        public ErrorOr<List<T>> Get<T, V>(string query, int id, Func<T, V, T> mapFunc)
        {
            var response = DataAccess.LoadData(query, new { Id = id }, mapFunc);
            if (response.Count < 1)
                return Errors.General.NotFound;
            return response;
        }

        public ErrorOr<Updated> Update<T>(string query, T data)
        {
            List<Error> errors = new();
            int rowsUpdated = 0;
            try
            {
                rowsUpdated = DataAccess.UpdateData(query, data);
            }
            catch
            {
                errors.Add(Errors.General.FailedToExecute);
            }
            if (rowsUpdated < 1)
                errors.Add(Errors.General.NoResult);
            if (errors.Count > 0)
                return errors;
            return Result.Updated;
        }

        //change this and use for filtering
        public ErrorOr<T> GetByData<T>(string query, T data)
        {
            try
            {
                return DataAccess.LoadData<T>(query, data).Last();
            }
            catch
            {
                return Errors.General.FailedToExecute;
            }
        }


    }
}
