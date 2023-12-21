using API.ServiceErrors;
using ErrorOr;
using static API.ServiceErrors.Errors;

namespace API.Models
{
    public class Report(int? id, DateTime dateFulfilled, string buyingRecordsLink, string? recieverReportLink, int? projectId)
    {
        public Report() : this(null, DateTime.Now, "", null, null) { }
        public  const int maxStringLength = 50;
        public const int minStringLength = 1;
        private static readonly string GetProjectsQuery = @"select id from projects";
        public int? Id { get; } = id;
        public DateTime DateFulfilled { get; } = dateFulfilled;
        public string BuyingRecordsLink { get; } = buyingRecordsLink;
        public string? RecieverReportLink { get; } = recieverReportLink;
        public int? ProjectId { get; } = projectId;
        public Project Project { get; set; }

        public static ErrorOr<Success> ValidateReport(UpsertReportsRequest report)
        {
            List<Error> errors = new();
            if (report.DateFulfilled.Year < 1800)
                errors.Add(Errors.Report.MissingDate);
            if (report.BuyingRecordsLink.Length is < minStringLength or > maxStringLength)
                errors.Add(Errors.Report.InvalidLink);
            if (report.RecieverReportLink != null && report.RecieverReportLink.Length is < minStringLength or > maxStringLength)
                errors.Add(Errors.Report.InvalidLink);
            if (report.ProjectId != null)
            {
                var projects = DataAccess.LoadData<Project>(GetProjectsQuery, null);
                bool doesIdExist = false;
                foreach (var project in projects)
                {
                    if (report.ProjectId == project.Id)
                    {
                        doesIdExist = true;
                        break;
                    }
                }
                if (!doesIdExist)
                    errors.Add(Errors.Report.InvalidProjectId);
            }
            if (errors.Count > 0)
                return errors;
            return Result.Success;
        }
        public static Report MapQuery(Report report, Project project)
        {
            report.Project = project; return report;
        }
        public static List<ReportsResponse> MapModel(List<Report> reports)
        {
            List<ReportsResponse> response = new();
            foreach (var item in reports)
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
        public static List<ReportsByProjectResponse> MapFilteredModel(List<Report> models)
        {
            List<ReportsByProjectResponse> response = [];
            List<int?> passedProjectIds = [];
            foreach (var model in models)
            {
                if (passedProjectIds.Contains(model.ProjectId))
                    continue;
                ReportsByProjectResponse responseItem;
                List<int?> ids = [];
                var project = model.Project;
                ProjectResponse projectResponse = new(project.Id, project.Name, project.TotalPrice, project.StartDate, project.FinishDate, project.Link, project.IsWithPartners, project.IsMilitary, project.TotalCollectedFunds);
                var filteredModels = models.FindAll(x => x.ProjectId == model.ProjectId);
                filteredModels.ForEach(x => ids.Add(x.Id));
                responseItem = new(ids, projectResponse, MapModel(filteredModels));
                response.Add(responseItem);
            }
            return response;
        }
    }
}
