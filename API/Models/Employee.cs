using API.ServiceErrors;
using ErrorOr;

namespace API.Models
{
    public class Employee(int? Id, string? FirstName, string? LastName, DateTime? BirthDate, DateTime? StartWorkDate, string? Position)
    {
        public const int minStringLength = 1;
        public const int maxStringLength = 50;
        public int? Id { get; } = Id;
        public string? FirstName { get; } = FirstName;
        public string? LastName { get; } = LastName;
        public DateTime? BirthDate { get; } = BirthDate;
        public DateTime? StartWorkDate { get; } = StartWorkDate;
        public string? Position { get; } = Position;
        public static ErrorOr<Employee> Create (string firstName, string lastName, string birthDate, string startWorkDate, string Position)
        {
            DateTime BirthDate, StartWorkDate;
            List<Error> errors = new();
            if (!DateTime.TryParse(startWorkDate, out StartWorkDate))
                errors.Add(Errors.Employee.InvalidDate);
            if (!DateTime.TryParse(birthDate, out BirthDate))
                errors.Add(Errors.Employee.InvalidDate);
            if (firstName.Length is < minStringLength or > maxStringLength || lastName.Length is < minStringLength or > maxStringLength || Position.Length is < minStringLength or > maxStringLength)
                errors.Add(Errors.Employee.InvalidInput);
            if (errors.Count > 0)
                return errors;
            return new Employee(null, firstName, lastName, BirthDate, StartWorkDate, Position);
        }
    }
}
