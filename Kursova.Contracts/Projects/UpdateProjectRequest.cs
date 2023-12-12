public record UpdateProjectRequest(
    int Id,
    string Name,
    double TotalPrice,
    DateTime StartDate,
    DateTime? FinishDate,
    string? Link,
    bool IsWithPartners,
    bool IsMilitary,
    double? TotalFundsCollected);