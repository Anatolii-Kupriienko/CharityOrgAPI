public record FilteredByProjectPartnerResponse(
    List<int?> Ids,
    ProjectResponse Project,
    List<UpdatePartnerRequestResponse> Partners);