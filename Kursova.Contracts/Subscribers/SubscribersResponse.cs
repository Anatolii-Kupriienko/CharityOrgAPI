using Kursova.Contracts.SupportDirections;

public record SubscribersResponse(
    int? Id,
    string FullName,
    double Amount,
    string Currency,
    DateTime DateSubscribed,
    SupportDirectionResponse SupportDirectionId);