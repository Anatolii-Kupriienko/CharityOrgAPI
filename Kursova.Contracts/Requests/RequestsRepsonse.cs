public record RequestsResponse(
    int? Id,
    ItemRecord Item,
    int amount,
    DateTime DateRecieved,
    string Urgency,
    string Requester,
    ReportsResponse? Report);