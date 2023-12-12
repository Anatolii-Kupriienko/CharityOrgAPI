using API.ServiceErrors;
using ErrorOr;

namespace API.Models
{
    public class Employee
    {
        public const int minStringLength = 1;
        public const int maxStringLength = 50;
        public int Id { get; }
        public string? FirstName { get; }
        public string? LastName { get; }
        public DateTime? BirthDate { get; }
        public DateTime? StartWorkDate { get; }
        public string? Position { get; }
        public Employee(int id, string? firstName, string? lastName, DateTime? birthDate, DateTime? startWorkDate, string? position)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            StartWorkDate = startWorkDate;
            Position = position;
        }

        private Employee( string firstName, string lastName, DateTime birthDate, DateTime startWorkDate, string position)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            StartWorkDate = startWorkDate;
            Position = position;
        }
        public static ErrorOr<Employee> Create (string firstName, string lastName, DateTime birthDate, DateTime startWorkDate, string Position)
        {
            if (firstName.Length is < minStringLength or > maxStringLength || lastName.Length is < minStringLength or > maxStringLength || Position.Length is < minStringLength or > maxStringLength)
                return Errors.Employee.InvalidInput;
            return new Employee(firstName, lastName, birthDate, startWorkDate, Position);
        }
    }
}
