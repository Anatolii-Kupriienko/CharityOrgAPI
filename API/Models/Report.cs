using API.ServiceErrors;
using ErrorOr;

namespace API.Models
{
    public class Report
    {
        public  const int maxStringLength = 50;
        public const int minStringLength = 1;
        private static readonly string GetProjectsQuery = @"select id from projects";
        public int? Id { get; }
        public DateTime DateFulfilled { get; }
        public string BuyingRecordsLink { get; }
        public string? RecieverReportLink { get; }
        public int? ProjectId { get; }
        public Project Project { get; set; }

        public Report(int? id, DateTime dateFulfilled, string buyingRecordsLink, string? recieverReportLink, int? projectId)
        {
            Id = id;
            DateFulfilled = dateFulfilled;
            BuyingRecordsLink = buyingRecordsLink;
            RecieverReportLink = recieverReportLink;
            ProjectId = projectId;
        }

        public Report(int id)
        {
            Id = id;
        }
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
    }
}
