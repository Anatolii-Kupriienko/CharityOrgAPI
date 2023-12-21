public record CreateRequestRequest(
    int ItemId,
    int Amount,
    DateTime? DateRecieved,
    string Urgency,
    string Requester);