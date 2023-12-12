using API.Models;
using API.ServiceErrors;
using ErrorOr;

namespace API.Services.SupportDirections
{
    public class SupportDirectionService : ISupportDirectionService
    {
        public static readonly string SelectQuery = @"select * from supportDirections where name = @Name and description = @Description and about = @About";
        public readonly string GetAllQuery = @"select * from supportDirections";
        public readonly string GetOneQuery = @"select * from supportDirection where id = @Id";
        public readonly string DeleteQuery = @"delete from supportDirection where id = @Id";
        public readonly string UpdateQuery = @"update supportDirection set name = @Name, description = @Description, about = @About where id = @Id";
        public readonly string InsertQuery = @"insert into supportDirections(name, description, about)values(@Name, @Description, @About)";
        
        public void CreateSupportDirection(SupportDirection data)
        {
            DataAccess.InsertData(InsertQuery, data);
        }

        public ErrorOr<Deleted> DeleteSupportDirection(int id)
        {
            int rowsDeleted =  DataAccess.UpdateData(DeleteQuery, new { Id = id });
            if (rowsDeleted < 1)
                return Errors.General.NoResult;
            return Result.Deleted;
        }

        public ErrorOr<List<SupportDirection>> GetSupportDirection(int id)
        {
            List<SupportDirection> response;
            List<Error> errors = new();
            if (id == 0)
            {
                response = DataAccess.LoadData<SupportDirection>(GetAllQuery, null);
                if (response.Count < 1)
                    errors.Add(Errors.General.TableEmpty);
            }
            else
                response = DataAccess.LoadData(GetOneQuery, new SupportDirection(id, "", null, null));
            if (response.Count < 1)
                errors.Add(Errors.General.NotFound);
            if (errors.Count > 0)
                return errors;
            return response;
        }

        public ErrorOr<Updated> UpdateSupportDirection(SupportDirection data)
        {
            int rowsUpdated = DataAccess.UpdateData(UpdateQuery, data);
            if (rowsUpdated < 1)
                return Errors.General.NoResult;
            return Result.Updated;
        }
        public static int? GetSupportDirectionId(SupportDirection data)
        {
            return DataAccess.LoadData(SelectQuery, data).First().Id;
        }
    }
}
