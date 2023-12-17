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
        public static ErrorOr<Employee> Create (string firstName, string lastName, DateTime birthDate, DateTime startWorkDate, string Position)
        {
            if (firstName.Length is < minStringLength or > maxStringLength || lastName.Length is < minStringLength or > maxStringLength || Position.Length is < minStringLength or > maxStringLength)
                return Errors.Employee.InvalidInput;
            return new Employee(null, firstName, lastName, birthDate, startWorkDate, Position);
        }
    }
}
