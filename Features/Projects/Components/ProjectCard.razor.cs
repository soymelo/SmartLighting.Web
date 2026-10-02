using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.Projects.Contracts.ViewModels;

namespace SmartLighting.Web.Features.Projects.Components;

public partial class ProjectCard
{
    [Parameter, EditorRequired]
    public required ProjectVMList Project { get; set; }

    [Parameter]
    public EventCallback<Guid> OnOpenDashboard { get; set; }
}
