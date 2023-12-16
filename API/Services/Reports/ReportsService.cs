using API.Models;
using API.ServiceErrors;
using ErrorOr;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Services.Reports
{
    public class ReportsService : IReportsService
    {
        private readonly string InsertQuery = @"insert into reports(dateFulfilled, buyingRecordsLink, recieverReportLink, projectId)values(@DateFulfilled, @BuyingRecordsLink, @RecieverReportLink, @ProjectId)";
        private readonly string DeleteQuery = @"delete from reports where id = @Id";
        private readonly string UpdateQuery = @"update reports set dateFulfilled = @DateFulfilled, buyingRecordsLink = @BuyingRecordsLink, recieverReportLink = @RecieverReportLink, projectId = @ProjectId where id = @Id";
        private readonly string GetOneQuery = @"select * from reports left join projects on projects.id = projectId where reports.id = @Id";
        private readonly string GetAllQuery = @"select * from reports left join projects on projects.id = projectId";
        private static readonly string SelectQuery = @"select * from reports where dateFulfilled = @DateFulfilled and buyingRecordsLink = @BuyingRecordsLink";

        public ErrorOr<Created> Create(UpsertReportsRequest requestData)
        {
            var validationResult = Report.ValidateReport(requestData);
            if (validationResult.IsError)
                return validationResult.Errors;
            DataAccess.InsertData(InsertQuery, requestData);
            return Result.Created;
        }

        public ErrorOr<Deleted> Delete(int id)
        {
            var rowsDeleted = DataAccess.UpdateData(DeleteQuery, new { Id = id });
            if (rowsDeleted < 1)
                return Errors.General.NoResult;
            return Result.Deleted;
        }

        public ErrorOr<List<ReportsResponse>> Get(int id)
        {
            if (id < 0)
                return Errors.General.InvalidId;
            List<ReportsResponse> response = new();
            List<Report> result;
            if (id == 0)
            {
                result = DataAccess.LoadData<Report, Project>(GetAllQuery, new Report(id), MapQuery);
                if (result.Count < 1)
                    return Errors.General.TableEmpty;
            }
            else
            {
                result = DataAccess.LoadData<Report, Project>(GetOneQuery, new Report(id), MapQuery);
                if(result.Count < 1)
                    return Errors.General.NotFound;
            }
            foreach (var item in result)
            {
                ReportsResponse responseItem;
                if (item.Project != null)
                {
                    var project = item.Project;
                    responseItem = new ReportsResponse(item.Id, item.DateFulfilled, item.BuyingRecordsLink, item.RecieverReportLink, new ProjectResponse(project.Id, project.Name, project.TotalPrice, project.StartDate, project.FinishDate, project.Link, project.IsWithPartners, project.IsMilitary, project.TotalCollectedFunds));
                }
                else
                    responseItem = new ReportsResponse(item.Id, item.DateFulfilled, item.BuyingRecordsLink, item.RecieverReportLink, null);
               
                response.Add(responseItem);
            }
            return response;
        }

        public ErrorOr<Updated> Update(UpsertReportsRequest requestData)
        {
            if (requestData.DateFulfilled.Year < 1800)
                return Errors.Report.MissingDate;
            var rowsUpdated = DataAccess.UpdateData(UpdateQuery, requestData);
            if (rowsUpdated < 1)
                return Errors.General.NoResult;
            return Result.Updated;
        }
        public static int? GetReportId(UpsertReportsRequest report)
        {
            return DataAccess.LoadData(SelectQuery, report).First().Id;
        }
        private Report MapQuery(Report report, Project project)
        {
            report.Project = project; return report;
        }
    }
}
