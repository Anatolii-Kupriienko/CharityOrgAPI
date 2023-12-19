using API.ServiceErrors;
using ErrorOr;
using System.Reflection.Metadata.Ecma335;

namespace API.Models
{
    public class Partner(int? Id, string OrgName, string Link)
    {
        public const int maxStringLength = 50;
        public int? Id { get; } = Id;
        public string OrgName { get; } = OrgName;
        public string Link { get; } = Link;

        public static ErrorOr<Success> ValidateStrings(string orgName, string link)
        {
            if (orgName.Length > maxStringLength || link.Length > maxStringLength)
                return Errors.Partner.InvalidInput;
            return Result.Success;       
        }
        public static ErrorOr<Success> ValidateUpdate(UpdatePartnerRequestResponse requestData)
        {
            if (requestData.Id > 0)
            {
                var validationResult = ValidateStrings(requestData.OrgName, requestData.Link);
                if (validationResult.IsError)
                    return validationResult.Errors;
                return Result.Success;
            }
            return Errors.General.InvalidId;
        }
    }
}
