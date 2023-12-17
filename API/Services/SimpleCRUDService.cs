using API.Models;
using API.ServiceErrors;
using API.Services.Interfaces;
using ErrorOr;

namespace API.Services
{
    public class SimpleCRUDService : ISimpleCRUDService
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
            List<Error> errors = new();
            var response = DataAccess.LoadData<T>(query, new { Id = id });
            if (response.Count < 1)
                errors.Add(Errors.General.NotFound);
            if (errors.Count > 0)
                return errors;
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
