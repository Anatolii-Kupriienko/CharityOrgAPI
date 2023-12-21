public record UpdateRequestRequest(
    int Id,
    int ItemId,
    int Amount,
    DateTime? DateRecieved,
    string Urgency,
    string Requester,
    int? ReportId);