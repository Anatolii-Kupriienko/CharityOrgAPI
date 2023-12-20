using API.ServiceErrors;
using ErrorOr;

namespace API.Models
{
    public class ProjectPartner(int? Id, int PartnerId, int ProjectId)
    {
        private static readonly string GetPartnersQuery = @"select * from partners where id = @Id";
        private static readonly string GetProjectsQuery = @"select * from projects where id = @Id";
        public int? Id { get; } = Id;
        public int PartnerId { get; } = PartnerId;
        public int ProjectId { get; } = ProjectId;
        public Partner? Partner { get; set; }
        public Project? Project { get; set; }

        public static ErrorOr<Success> ValidateRequest(int? id, int partnerId, int projectId)
        {
            List<Error> errors = new();
            var partner = DataAccess.LoadData<Partner>(GetPartnersQuery, new { Id = partnerId });
            var project = DataAccess.LoadData<Project>(GetProjectsQuery, new { Id = projectId });
            if (id is not null and < 1)
                errors.Add(Errors.General.InvalidId);
            if (partner.Count == 0)
                errors.Add(Errors.ProjectPartner.InvalidPartner);
            if (project.Count == 0)
                errors.Add(Errors.ProjectPartner.InvalidProejct);
            if(errors.Count > 0)
                return errors;
            return Result.Success;
        }
        public static ProjectPartner MapQuery(ProjectPartner projectPartner, Partner partner, Project project)
        {
            projectPartner.Partner = partner;
            projectPartner.Project = project;
            return projectPartner;
        }
        public static List<ProjectPartnerResponse> MapModel(List<ProjectPartner> models)
        {
            List<ProjectPartnerResponse> response = new();
            foreach (var model in models)
            {
                ProjectPartnerResponse responseItem;
                var project = model.Project;
                var partner = model.Partner;
                ProjectResponse responseProject = new(project.Id, project.Name, project.TotalPrice, project.StartDate, project.FinishDate, project.Link, project.IsWithPartners, project.IsMilitary, project.TotalCollectedFunds);
                UpdatePartnerRequestResponse responsePartner = new(partner.Id, partner.OrgName, partner.Link);
                responseItem = new(model.Id, responsePartner, responseProject);
                response.Add(responseItem);
            }
            return response;
        }
        public static List<FilteredByProjectPartnerResponse> MapFilteredByProjectModel(List<ProjectPartner> models)
        {
            List<FilteredByProjectPartnerResponse> response = new();
            List<int?> passedProjectIds = new();
            foreach (var model in models)
            {
                if (passedProjectIds.Contains(model.Project.Id))
                    continue;
                FilteredByProjectPartnerResponse responseItem;
                Project project = model.Project;
                var projectModels = models.FindAll(x => x.Project.Id == project.Id);
                List<int?> ids = new();
                List<UpdatePartnerRequestResponse> projectPartners = new();
                projectModels.ForEach(item => {
                    projectPartners.Add(new UpdatePartnerRequestResponse
                    (item.Partner.Id, item.Partner.OrgName, item.Partner.Link));
                    ids.Add(item.Id);
                });
                responseItem = new(ids, new ProjectResponse(project.Id, project.Name, project.TotalPrice, project.StartDate, project.FinishDate, project.Link, project.IsWithPartners, project.IsMilitary, project.TotalCollectedFunds), projectPartners);
                response.Add(responseItem);
                passedProjectIds.Add(project.Id);
            }
            return response;
        }
        public static List<FilteredByPartnerProjectPartnerResponse> MapFileteredPartnerModel(List<ProjectPartner> models)
        {
            List<FilteredByPartnerProjectPartnerResponse> response = new();
            List<int?> passedPartnerIds = new();
            foreach (var model in models)
            {
                Partner partner = model.Partner;
                if (passedPartnerIds.Contains(partner.Id))
                    continue;
                FilteredByPartnerProjectPartnerResponse responseItem;
                var partnerModels = models.FindAll(x => x.PartnerId == model.PartnerId);
                List<int?> ids = new();
                List<ProjectResponse> partnerProjects = new();
                partnerModels.ForEach(item =>
                {
                    var project = item.Project;
                    partnerProjects.Add(new ProjectResponse(project.Id, project.Name, project.TotalPrice, project.StartDate, project.FinishDate, project.Link, project.IsWithPartners, project.IsMilitary, project.TotalCollectedFunds));
                    ids.Add(item.Id);
                });
                responseItem = new(ids, new UpdatePartnerRequestResponse(partner.Id, partner.OrgName, partner.Link), partnerProjects);
                response.Add(responseItem);
                passedPartnerIds.Add(partner.Id);
            }
            return response;
        }
    }
}
