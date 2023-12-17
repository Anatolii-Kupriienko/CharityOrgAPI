public record DonationsRequest(
    int? Id,
    string Sender,
    double Amount,
    string Currency,
    int SupportDirectionId,
    DateTime Date);