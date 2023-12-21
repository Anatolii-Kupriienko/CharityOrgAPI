using Kursova.Contracts.SupportDirections;

namespace Kursova.Contracts.Donations
{
    public record DonationsForSupportDirectionResponse(
        List<int?> Ids,
        SupportDirectionResponse SupportDirection,
        List<DonationsResponse> Donations);
}