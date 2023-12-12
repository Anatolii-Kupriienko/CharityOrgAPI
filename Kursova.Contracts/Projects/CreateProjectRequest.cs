public record CreateProjectRequest(
    string Name,
    double TotalPrice,
    DateTime StartDate,
    string? Link,
    bool IsWithPartners,
    bool IsMilitary);