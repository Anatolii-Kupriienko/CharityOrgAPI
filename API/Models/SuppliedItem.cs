using API.ServiceErrors;
using ErrorOr;

namespace API.Models
{
    public class SuppliedItem(int? Id, string? Name, int AmountSupplied, string? GeneralName)
    {
        public SuppliedItem() : this(null, null, 0, null) { }
        public const int maxNameLength = 150;
        public const int maxGeneralNameLength = 100;
        public int? Id { get; } = Id;
        public string? Name { get; } = Name;
        public int AmountSupplied {  get; } = AmountSupplied;
        public string? GeneralName { get; } = GeneralName;

        public static ErrorOr<SuppliedItem> Create(int? id, string? name, int amount, string? generalName)
        {
            List<Error> errors = new();
            if (name != null && name.Length > maxNameLength)
                errors.Add(Errors.SuppliedItem.InvalidName);
            if (generalName != null && generalName.Length > maxGeneralNameLength)
                errors.Add(Errors.SuppliedItem.InvalidGeneralName);
            if (amount < 0)
                errors.Add(Errors.SuppliedItem.InvalidAmount);
            if (errors.Count > 0)
                return errors;
            return new SuppliedItem(id, name, amount, generalName);
        }
    }
}
