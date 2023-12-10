public record CreateEmployeeRequest(
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    DateOnly StartWorkDate,
    string Position);
