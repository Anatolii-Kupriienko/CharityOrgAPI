using API.ServiceErrors;
using ErrorOr;

namespace API.Models
{
    public class SupportDirection(int? Id, string Name, string? Description, string? About)
    {
        public const int minNameLength = 1;
        public const int maxNameLength = 50;
        public const int maxDescLength = 150;
        public int? Id { get; } = Id;
        public string Name { get; } = Name;
        public string? Description { get; } = Description;
        public string? About { get; } = About;
        
        public static ErrorOr<SupportDirection> Create(string name, string? description, string? about, int? id)
        {
            List<Error> errors = new();
            if (description != null)
                if (description.Length is > maxDescLength)
                    errors.Add(Errors.SupportDirection.InvalidDescription);
            if(name.Length is < minNameLength or > maxNameLength)
                errors.Add(Errors.SupportDirection.InvalidName);
            if (errors.Count > 0)
                return errors;
            return new SupportDirection(id, name, description, about);
        }
    }
}
