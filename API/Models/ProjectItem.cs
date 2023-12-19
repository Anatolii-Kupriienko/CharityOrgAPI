using API.Controllers;
using API.ServiceErrors;
using ErrorOr;

namespace API.Models
{
    public class ProjectItem(int ItemId, int ProjectId)
    {
        private static readonly string GetItemsQuery = @"select id from suppliedItems";
        private static readonly string GetProjectsQuery = @"select id from projects";
        public int ItemId { get; } = ItemId;
        public int ProjectId { get; } = ProjectId;
        public SuppliedItem? Item { get; set; }
        public Project? Project { get; set; }

        public static ErrorOr<Success> ValidateRequest(UpsertProjectItemsReuqest requestData)
        {
            var items = DataAccess.LoadData<SuppliedItem>(GetItemsQuery, null);
            var projects = DataAccess.LoadData<Project>(GetProjectsQuery, null);
            List<Error> errors = new();
            if (projects.Find(project => project.Id == requestData.ProjectId) == null)
                errors.Add(Errors.ProjectItem.InvalidProject);
            if (items.Find(item => item.Id == requestData.ItemId) == null)
                errors.Add(Errors.ProjectItem.InvalidItem);
            if (errors.Count > 0)
                return errors;
            return Result.Success;
        }
        public static ProjectItem mapQuery(ProjectItem projectItem, SuppliedItem item, Project project)
        {
            projectItem.Project = project;
            projectItem.Item = item;
            return projectItem;
        }
        public static List<ProjectItemsResponse> MapModel(List<ProjectItem> models)
        {
            List<ProjectItemsResponse> response = new();
            foreach (var model in models)
            {
                ProjectItemsResponse responseProjectItem;
                var project = model.Project;
                var item = model.Item;
                ItemRecord responseItem = new(item.Id, item.Name, item.AmountSupplied, item.GeneralName);
                ProjectResponse responseProject = new(project.Id, project.Name, project.TotalPrice, project.StartDate, project.FinishDate, project.Link, project.IsWithPartners, project.IsMilitary, project.TotalCollectedFunds);
                responseProjectItem = new(responseItem, responseProject);
                response.Add(responseProjectItem);
            }
            return response;
        }
        public static List<FilteredProjectItemResponse> MapFilteredModel(List<ProjectItem> models)
        {
            List<FilteredProjectItemResponse> response = new();
            List<int?> passedProjectIds = new();
            foreach (var model in models)
            {
                if (passedProjectIds.Contains(model.Project.Id))
                    continue;
                FilteredProjectItemResponse responseItem;
                Project project = model.Project;
                var projectModels = models.FindAll(x => x.Project.Id == project.Id);
                List<ItemRecord> projectItems = new();
                projectModels.ForEach(item => projectItems.Add(new ItemRecord(item.Item.Id, item.Item.Name, item.Item.AmountSupplied, item.Item.GeneralName)));
                responseItem = new(new ProjectResponse(project.Id, project.Name, project.TotalPrice, project.StartDate, project.FinishDate, project.Link, project.IsWithPartners, project.IsMilitary, project.TotalCollectedFunds), projectItems);
                response.Add(responseItem);
                passedProjectIds.Add(project.Id);
            }
            return response;
        }
    }
}
