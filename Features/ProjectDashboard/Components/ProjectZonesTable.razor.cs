using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

namespace SmartLighting.Web.Features.ProjectDashboard.Components;

public partial class ProjectZonesTable
{
    [Parameter, EditorRequired]
    public required IReadOnlyCollection<ProjectZoneVMList> Zones { get; set; }
}
