using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

namespace SmartLighting.Web.Features.ProjectDashboard.Components;

public partial class ProjectRecentSensorEventsTable
{
    [Parameter, EditorRequired]
    public required IReadOnlyCollection<ProjectSensorEventVMList> Events { get; set; }
}
