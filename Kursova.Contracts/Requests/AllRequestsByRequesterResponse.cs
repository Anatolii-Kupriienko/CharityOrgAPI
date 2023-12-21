public record AllRequestsByRequesterResponse(
    List<int?> RequestsIds,
    string Requester,
    List<RequestsResponse> Requests);