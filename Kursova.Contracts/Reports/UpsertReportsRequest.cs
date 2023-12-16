public record UpsertReportsRequest(
    int? Id,
    DateTime DateFulfilled,
    string BuyingRecordsLink,
    string? RecieverReportLink,
    int? ProjectId);