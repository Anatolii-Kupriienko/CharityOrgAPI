using API.ServiceErrors;
using ErrorOr;

namespace API.Models
{
    public class Request(int? Id, int ItemId, int Amount, DateTime? DateRecieved, string Urgency, string Requester, int? ReportId)
    {
        public const int maxStringLength = 50;
        public static readonly List<string> PossibleUrgencyValues = ["Негайна", "Висока", "Середня", "Низька"];
        private static readonly string GetItemsQuery = @"select id from suppliedItems where id = @Id";
        private static readonly string getReportsQuery = @"select id from reports where id = @Id";
        public int? Id { get; } = Id;
        public int ItemId { get; } = ItemId;
        public int Amount { get; } = Amount;
        public DateTime DateRecieved { get; } = DateRecieved ?? DateTime.Now;
        public string Urgency { get; } = Urgency;
        public string Requester { get; } = Requester;
        public int? ReportId { get; } = ReportId;
        private SuppliedItem? Item { get; set; }
        private Report? Report { get; set; }

        public static ErrorOr<Request> Create(CreateRequestRequest requestData)
        {
            List<Error> errors = new();
            var itemValidationResult = ValidateItemId(requestData.ItemId);
            if (itemValidationResult.IsError)
                errors.Add(itemValidationResult.FirstError);
            if (requestData.Amount <= 0)
                errors.Add(Errors.Request.InvalidAmount);
            if (requestData.DateRecieved > DateTime.Now)
                errors.Add(Errors.Request.InvalidDate);
            if (!PossibleUrgencyValues.Contains(requestData.Urgency))
                errors.Add(Errors.Request.InvalidUrgency);
            if (requestData.Requester.Length > maxStringLength)
                errors.Add(Errors.Request.InvalidRequester);
            if (errors.Count > 0)
                return errors;
            return new Request(null, requestData.ItemId, requestData.Amount, requestData.DateRecieved, requestData.Urgency, requestData.Requester, null);
        }
        public static ErrorOr<Success> Validate(UpdateRequestRequest requestData)
        {
            List<Error> errors = new();
            var itemValidationResult = ValidateItemId(requestData.ItemId);
            var reportValidationResult = ValidateReport(requestData.ReportId);
            if (requestData.Id <= 0)
                errors.Add(Errors.General.InvalidId);
            if (reportValidationResult.IsError && requestData.ReportId != null)
                errors.Add(Errors.Request.ReportDoesntExist);
            if (itemValidationResult.IsError)
                errors.Add(itemValidationResult.FirstError);
            if (requestData.Amount <= 0)
                errors.Add(Errors.Request.InvalidAmount);
            if (requestData.DateRecieved == null || requestData.DateRecieved > DateTime.Now)
                errors.Add(Errors.Request.InvalidDate);
            if (!PossibleUrgencyValues.Contains(requestData.Urgency))
                errors.Add(Errors.Request.InvalidUrgency);
            if (requestData.Requester.Length > maxStringLength)
                errors.Add(Errors.Request.InvalidRequester);
            if (errors.Count > 0)
                return errors;
            return Result.Success;
        }
        public static ErrorOr<Success> ValidateReport(int? id)
        {
            var queryResult = DataAccess.LoadData <Report>(getReportsQuery, new { Id = id });
            if (queryResult.Count == 0)
                return Errors.Request.ReportDoesntExist;
            return Result.Success;
        }
        private static ErrorOr<Success> ValidateItemId(int id)
        {
            var queryResult = DataAccess.LoadData<SuppliedItem>(GetItemsQuery, new { Id = id });
            if (queryResult.Count == 0)
                return Errors.Request.ItemDoesntExist;
            return Result.Success;
        }
        public static Request MapQuery(Request request, SuppliedItem item, Report? report)
        {
            request.Item = item;
            request.Report = report;
            return request;
        }
        public static List<RequestsResponse> MapModel(List<Request> models)
        {
            List<RequestsResponse> response = new();
            foreach (var model in models)
            {
                RequestsResponse responseItem;
                ItemRecord itemResponse = new(model.Item.Id, model.Item.Name, model.Item.AmountSupplied, model.Item.GeneralName);
                var report = model.Report;
                ReportsResponse reportResponse = null;
                ProjectResponse projectResponse = report?.Project != null ? new(report.Project.Id, report.Project.Name, report.Project.TotalPrice, report.Project.StartDate, report.Project.FinishDate, report.Project.Link, report.Project.IsWithPartners, report.Project.IsMilitary, report.Project.TotalCollectedFunds) : null;
                if (report != null)
                    reportResponse = new(report.Id, report.DateFulfilled, report.BuyingRecordsLink, report.RecieverReportLink, projectResponse);
                responseItem = new(model.Id, itemResponse, model.Amount, model.DateRecieved, model.Urgency, model.Requester, reportResponse);
                response.Add(responseItem);
            }
            return response;
        }
        public static List<AllRequestsByRequesterResponse> ModelFilteredModel(List<Request> models)
        {
            List<AllRequestsByRequesterResponse> response = new();
            List<string> passedRequesters = new();
            foreach (var model in models)
            {
                if (passedRequesters.Contains(model.Requester))
                    continue;
                List<int?> ids = new();
                List<Request> requesterModels = models.FindAll(r => r.Requester == model.Requester);
                var mappedRequesterModels = MapModel(requesterModels);
                mappedRequesterModels.ForEach(m => ids.Add(m.Id));
                response.Add(new(ids, model.Requester, mappedRequesterModels));
                passedRequesters.Add(model.Requester);
            }
            return response;
        } 
    }
}
