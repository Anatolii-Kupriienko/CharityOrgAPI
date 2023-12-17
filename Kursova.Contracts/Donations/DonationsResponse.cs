using Kursova.Contracts.SupportDirections;

public record DonationsResponse(
    int? Id,
    string Sender,
    double Amount,
    string Currency,
    SupportDirectionResponse? SupportDirection,
    DateTime Date);