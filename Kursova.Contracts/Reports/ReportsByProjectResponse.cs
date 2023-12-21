public record ReportsByProjectResponse(
    List<int?> Ids,
    ProjectResponse Project,
    List<ReportsResponse> Reports);