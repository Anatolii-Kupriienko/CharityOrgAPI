public record EmployeeResponse(
    int Id,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    DateOnly StartWorkDate,
    string Position);