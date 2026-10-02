using SmartLighting.Web.Features.Projects.Contracts.Dtos;
using SmartLighting.Web.Features.Projects.Contracts.ViewModels;
using SmartLighting.Web.Infrastructure;

namespace SmartLighting.Web.Features.Projects.Application;

public class ProjectService(IApiClient apiClient) : ServiceBase
{
    public async Task<ServiceResult<IReadOnlyCollection<ProjectVMList>>> GetProjectsAsync()
    {
        var response = await apiClient.GetAsync<GetProjectsDto>("/projects");

        return FromApiResponse(
            response,
            data => (IReadOnlyCollection<ProjectVMList>)data.Projects
                .Select(project => new ProjectVMList(project.ProjectId, project.Name))
                .ToArray());
    }
}
