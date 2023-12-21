using API.ServiceErrors;
using ErrorOr;
using Kursova.Contracts.Donations;
using Kursova.Contracts.SupportDirections;
using static API.ServiceErrors.Errors;

namespace API.Models
{
    public class Donation(int? Id, string Sender, double Amount, string Currency, int SupportDirectionId, DateTime Date)
    {
        private static readonly string GetSuppDirectionsQuery = "select id from supportDirections";
        public const int maxSenderLength = 50;
        private static readonly string[] possibleCurrencies = ["USD", "UAH", "EUR"];
        public int? Id { get; } = Id;
        public string Sender { get; } = Sender;
        public double Amount { get; } = Amount;
        public string Currency { get; } = Currency;
        public int SupportDirectionId { get; } = SupportDirectionId;
        public SupportDirection? SupportDirection { get; set; }
        public DateTime Date { get; } = Date;

        public static ErrorOr<Success> ValidateDonation(DonationsRequest donationsRequest)
        {
            List<Error> errors = [];
            if (donationsRequest.Sender.Length > maxSenderLength)
                errors.Add(Errors.Donation.InvalidSender);
            if (!possibleCurrencies.Contains(donationsRequest.Currency.ToUpper()))
                errors.Add(Errors.Donation.InvalidCurrency);
            if (donationsRequest.Amount <= 0)
                errors.Add(Errors.Donation.InvalidAmount);
            if (donationsRequest.Date.Year < 1800)
                errors.Add(Errors.Donation.InvalidDate);
            var supportDirections = DataAccess.LoadData<SupportDirection>(GetSuppDirectionsQuery, null);
            bool doesIdExist = false;
            foreach (var supportDirection in supportDirections)
            {
                if (donationsRequest.SupportDirectionId == supportDirection.Id)
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
        public static Donation MapQuery(Donation donation, SupportDirection supportDirection)
        {
            donation.SupportDirection = supportDirection; return donation;
        }
        public static List<DonationsResponse> MapModel(List<Donation> donations)
        {
            List<DonationsResponse> response = [];
            foreach (var item in donations)
            {
                DonationsResponse responseItem;
                var project = item.SupportDirection;
                var supportDirection = new SupportDirectionResponse(item.SupportDirection.Id, item.SupportDirection.Name, item.SupportDirection.Description, item.SupportDirection.About);
                responseItem = new DonationsResponse(item.Id, item.Sender, item.Amount, item.Currency, 
                    supportDirection, item.Date);

                response.Add(responseItem);
            }
            return response;
        }
        public static List<DonationsForSupportDirectionResponse> MapFilteredModel(List<Donation> models)
        {
            List<DonationsForSupportDirectionResponse> response = [];
            List<int?> passedSupportDirections = [];
            foreach (var model in models)
            {
                if (passedSupportDirections.Contains(model.SupportDirectionId))
                    continue;
                DonationsForSupportDirectionResponse responseItem;
                List<int?> ids = [];
                SupportDirectionResponse supportDirectionResponse = new(model.SupportDirection.Id, model.SupportDirection.Name, model.SupportDirection.Description, model.SupportDirection.About);
                var filteredModels = models.FindAll(x => x.SupportDirectionId == model.SupportDirectionId);
                filteredModels.ForEach(x => ids.Add(x.Id));
                responseItem = new(ids, supportDirectionResponse, MapModel(filteredModels));
                response.Add(responseItem);
                passedSupportDirections.Add(model.SupportDirectionId);
            }
            return response;
        }

    }
}
