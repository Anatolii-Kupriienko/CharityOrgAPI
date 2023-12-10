namespace API.Models
{
    public class Employee
    {
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime BirthDate { get; }
        public DateTime StartWorkDate { get; }
        public string Position { get; }

        public Employee( string firstName, string lastName, DateTime birthDate, DateTime startWorkDate, string position)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            StartWorkDate = startWorkDate;
            Position = position;
        }
    }
}
