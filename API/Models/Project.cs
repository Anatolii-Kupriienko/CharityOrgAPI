using API.ServiceErrors;
using ErrorOr;

namespace API.Models
{
    public class Project
    {
        public const int minStringLength = 1;
        public const int maxNameLength = 50;
        public const int maxLinkLength = 100;
        public int? Id { get; }
        public string Name { get; }
        public double TotalPrice { get; }
        public DateTime StartDate { get; }
        public DateTime? FinishDate { get; }
        public string? Link { get; }
        public bool IsWithPartners { get; }
        public bool IsMilitary { get; }
        public double? TotalCollectedFunds { get; }

        public Project(int? id, string name, double totalPrice, DateTime startDate, DateTime? finishDate, string? link, bool isWithPartners, bool isMilitary, double? totalCollectedFunds)
        {
            Id = id;
            Name = name;
            TotalPrice = totalPrice;
            StartDate = startDate;
            FinishDate = finishDate;
            Link = link;
            IsWithPartners = isWithPartners;
            IsMilitary = isMilitary;
            TotalCollectedFunds = totalCollectedFunds;
        }
        public Project(int id)
        {
            Id = id;
            Name = "";
            TotalPrice = 0;
            StartDate = DateTime.Now;
            IsWithPartners = false;
            IsMilitary = false;
        }
        public static ErrorOr<Project> Create(int? id, string name, double totalPrice, DateTime startDate, DateTime? finishDate, string? link, bool isWithPartners, bool isMilitary, double? totalFundsCollected)
        {
            List<Error> errors = new();
            if (startDate.Year < 1800)
                errors.Add(Errors.Project.MissingValues);
            if (name.Length is > maxNameLength  or < 1)
                errors.Add(Errors.Project.InvalidName);
            if (link != null && link.Length > maxLinkLength)
                errors.Add(Errors.Project.InvalidLink);
            if (totalPrice < 0)
                errors.Add(Errors.Project.InvalidPrice);
            if (errors.Count > 0)
                return errors;
            return new Project(id, name, totalPrice, startDate, finishDate, link, isWithPartners, isMilitary, totalFundsCollected);
        }
    }
}
