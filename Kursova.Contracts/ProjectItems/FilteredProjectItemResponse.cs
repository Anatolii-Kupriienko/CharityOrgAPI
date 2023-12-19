public record FilteredProjectItemResponse(
    ProjectResponse Project,
    List<ItemRecord> Items);