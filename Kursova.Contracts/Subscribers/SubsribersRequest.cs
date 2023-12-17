public record SubscribersRequest(
    int? Id,
    string FullName,
    double Amount,
    string Currency,
    DateTime DateSubscribed,
    int SupportDirectionId);