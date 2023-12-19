using API.ServiceErrors;
using ErrorOr;
using Kursova.Contracts.SupportDirections;
using static API.ServiceErrors.Errors;

namespace API.Models
{
    public class Subscriber(int? Id, string FullName, double Amount, string Currency, DateTime DateSubscribed, int? SupportDirectionId)
    {
        private static readonly string GetSuppDirectionsQuery = "select id from supportDirections";
        private static string[] possibleCurrencies = { "USD", "UAH", "EUR" };
        public const int maxNameLength = 50;
        public int? Id { get; } = Id;
        public string FullName { get; } = FullName;
        public double Amount { get; } = Amount;
        public string Currency { get; } = Currency;
        public DateTime DateSubscribed { get; } = DateSubscribed;
        public SupportDirection? SupportDirection { get; set; }
        public int? SupportDirectionId { get; } = SupportDirectionId;

        public static ErrorOr<Success> Validate(SubscribersRequest subscribersRequest)
        {
            List<Error> errors = new();
            
            if (subscribersRequest.FullName.Length > maxNameLength)
                errors.Add(Errors.Subscriber.InvalidName);
            if (subscribersRequest.Amount <= 0)
                errors.Add(Errors.Subscriber.InvalidAmount);
            if (subscribersRequest.DateSubscribed.Year < 1800)
                errors.Add(Errors.Subscriber.InvalidDate);
            var supportDirections = DataAccess.LoadData<SupportDirection>(GetSuppDirectionsQuery, null);
            bool doesIdExist = false;
            foreach (var supportDirection in supportDirections)
            {
                if (subscribersRequest.SupportDirectionId == supportDirection.Id)
                {
                    doesIdExist = true;
                    break;
                }
            }
            if (!doesIdExist)
                errors.Add(Errors.Donation.InvalidSupportDirectionId);
            if (errors.Count > 0)
                return errors;
            return Result.Success;
        }
        public static Subscriber MapQuery(Subscriber subscriber, SupportDirection supportDirection)
        {
            subscriber.SupportDirection = supportDirection; return subscriber;
        }
        public static List<SubscribersResponse> MapModel(List<Subscriber> subscribers)
        {
            List<SubscribersResponse> response = new();
            foreach (var item in subscribers)
            {
                SubscribersResponse responseItem;
                var supportDirection = new SupportDirectionResponse(item.SupportDirection.Id, item.SupportDirection.Name, item.SupportDirection.Description, item.SupportDirection.About);
                responseItem = new SubscribersResponse(item.Id, item.FullName, item.Amount, item.Currency,
                    item.DateSubscribed, supportDirection);

                response.Add(responseItem);
            }
            return response;
        }

    }
}
