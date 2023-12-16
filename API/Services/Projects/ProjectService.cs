using API.Models;
using API.ServiceErrors;
using ErrorOr;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private static readonly string SelectQuery = @"select * from projects where name = @Name and totalPrice = @TotalPrice and startDate = @StartDate";
        private readonly string InsertQuery = @"insert into projects(name, totalPrice, startDate, finishDate, link, isWithPartners, isMilitary, totalCollectedFunds)values(@Name, @TotalPrice, @StartDate, @FinishDate, @Link, @IsWithPartners, @IsMilitary, @TotalCollectedFunds)";
        private readonly string DeleteQuery = @"delete from projects where id = @Id";
        private readonly string GetOneQuery = @"select * from projects where id = @Id";
        private readonly string GetAllQuery = @"select * from projects";
        private readonly string UpdateQeury = @"update projects set name = @Name, totalPrice = @TotalPrice, startDate = @StartDate, finishDate = @FinishDate, link = @Link, isWithPartners = @IsWithPartners, isMilitary = @IsMilitary, totalCollectedFunds = @TotalCollectedFunds where id = @Id";
        
        public ErrorOr<Created> CreateProject(Project requestData)
        {
            try
            {
                DataAccess.InsertData(InsertQuery, requestData);
                return Result.Created;
            }
            catch
            {
                return Errors.Project.DuplicateName;
            }
        }

        public ErrorOr<Deleted> DeleteProject(int id)
        {
            int rowsDeleted = DataAccess.UpdateData(DeleteQuery, new { Id = id });
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
            List<ErrorOr.Error> errors = new();
            int rowsUpdated = 0;
            try
            {
                rowsUpdated = DataAccess.UpdateData(UpdateQeury, requestData);
            }
            catch
            {
                errors.Add(Errors.Project.DuplicateName);
            }
            if (rowsUpdated < 1)
                errors.Add(Errors.General.NoResult);
            if (errors.Count > 0)
                return errors;
            return Result.Updated;
        }
        public static int? GetProjectId(Project data)
        {
            return DataAccess.LoadData(SelectQuery, data).First().Id;
        }
    }
}
