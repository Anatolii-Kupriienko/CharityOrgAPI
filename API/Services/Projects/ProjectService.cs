using API.Models;
using API.ServiceErrors;
using ErrorOr;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private static readonly string SelectQuery = @"select * from projects where name = @Name and startDate = @StartDate and finishDate = @FinishDate";
        private readonly string InsertQuery = @"insert into projects(name, totalPrice, startDate, finishDate, link, isWithPartners, isMilitary, totalCollectedFunds)values(@Name, @TotalPrice, @StartDate, @FinishDate, @Link, @IsWithPartners, @IsMilitary, @TotalCollectedFunds)";
        private readonly string DeleteQuery = @"delete from projects where id = @Id";
        private readonly string GetOneQuery = @"select * from projects where id = @Id";
        private readonly string GetAllQuery = @"select * from projects";
        private readonly string UpdateQeury = @"update projects set name = @Name, totalPrice = @TotalPrice, startDate = @StartDate, finishDate = @FinishDate, link = @Link, isWithPartners = @IsWithPartners, isMilitary = @IsMilitary where id = @Id";
        
        public void CreateProject(Project requestData)
        {
            DataAccess.InsertData(InsertQuery, requestData);
        }

        public ErrorOr<Deleted> DeleteProject(int id)
        {
            int rowsDeleted = DataAccess.UpdateData(DeleteQuery, id);
            if (rowsDeleted < 1)
                return Errors.General.NoResult;
            return Result.Deleted;
        }

        public ErrorOr<List<Project>> GetProject(int id)
        {
            List<Project> response;
            List<ErrorOr.Error> errors = new();
            if (id == 0)
            {
                response = DataAccess.LoadData<Project>(GetAllQuery, null);
                if (response.Count < 1)
                    errors.Add(Errors.General.TableEmpty);
            }
            else
                response = DataAccess.LoadData(GetOneQuery, new Project(id));
            if (response.Count < 1)
                errors.Add(Errors.General.NotFound);
            if (errors.Count > 0)
                return errors;
            return response;
        }

        public ErrorOr<Updated> UpdateProject(Project requestData)
        {
            int rowsUpdated = DataAccess.UpdateData(UpdateQeury, requestData);
            if (rowsUpdated < 1)
                return Errors.General.NoResult;
            return Result.Updated;
        }
        public static int? GetProjectId(Project data)
        {
            return DataAccess.LoadData(SelectQuery, data).First().Id;
        }
    }
}
