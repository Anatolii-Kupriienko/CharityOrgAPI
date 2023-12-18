using API.ServiceErrors;
using ErrorOr;

namespace API.Models
{
    public class Project(int? Id, string Name, double TotalPrice, DateTime StartDate, DateTime? FinishDate, string? Link, bool IsWithPartners, bool IsMilitary, double? TotalCollectedFunds)
    {
        public const int minStringLength = 1;
        public const int maxNameLength = 50;
        public const int maxLinkLength = 100;
        public int? Id { get; } = Id;
        public string Name { get; } = Name;
        public double TotalPrice { get; } = TotalPrice;
        public DateTime StartDate { get; } = StartDate;
        public DateTime? FinishDate { get; } = FinishDate;
        public string? Link { get; } = Link;
        public bool IsWithPartners { get; } = IsWithPartners;
        public bool IsMilitary { get; } = IsMilitary;
        public double? TotalCollectedFunds { get; } = TotalCollectedFunds;

        public Project() : this(null, "", 0, DateTime.Now, null, null, false, false, null) { }

        public static ErrorOr<Project> Create(int? id, string name, double totalPrice, DateTime startDate, DateTime? finishDate, string? link, bool isWithPartners, bool isMilitary, double? totalFundsCollected)
        {
            List<Error> errors = new();
            if (startDate.Year < 1800)
                errors.Add(Errors.Project.MissingValues);
            if (name.Length is > maxNameLength  or < 1)
                errors.Add(Errors.Project.InvalidName);
            if (link != null && link.Length > maxLinkLength)
                errors.Add(Errors.Project.InvalidLink);
            if (totalPrice <= 0 || (totalFundsCollected != null && totalFundsCollected <=0))
                errors.Add(Errors.Project.InvalidPrice);
            if (errors.Count > 0)
                return errors;
            return new Project(id, name, totalPrice, startDate, finishDate, link, isWithPartners, isMilitary, totalFundsCollected);
        }
    }
}
