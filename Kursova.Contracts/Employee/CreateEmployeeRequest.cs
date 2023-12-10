public record CreateEmployeeRequest(
    string FirstName,
    string LastName,
    DateTime BirthDate,
    DateTime StartWorkDate,
    string Position);
