
public record ReportsResponse(
    int? Id,
    DateTime DateFulfilled,
    string BuyingRecordsLink,
    string? RecieverReportLink,
    ProjectResponse? project);