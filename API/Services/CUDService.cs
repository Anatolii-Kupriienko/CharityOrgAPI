using API.Models;
using API.ServiceErrors;
using API.Services.Interfaces;
using ErrorOr;

namespace API.Services
{
    public class CUDService : ICUDService
    {
        public ErrorOr<Created> Create<T>(string query, T requestData)
        {
            try
            {
                DataAccess.InsertData(query, requestData);
                return Result.Created;
            }
            catch
            {
                return Errors.General.FailedToExecute;
            }
            
        }

        public ErrorOr<Deleted> Delete(string query, int id)
        {
            var rowsDeleted = DataAccess.UpdateData(query, new { Id = id });
            if (rowsDeleted < 1)
                return Errors.General.NoResult;
            return Result.Deleted;
        }

        public ErrorOr<Updated> Update<T>(string query, T requestData)
        {
            var rowsUpdated = DataAccess.UpdateData(query, requestData);
            if (rowsUpdated < 1)
                return Errors.General.NoResult;
            return Result.Updated;
        }
    }
}
