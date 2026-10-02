using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

namespace SmartLighting.Web.Features.ProjectDashboard.Components;

public partial class ProjectLuminaireStatesTable
{
    [Parameter, EditorRequired]
    public required IReadOnlyCollection<ProjectLuminaireVMList> Luminaires { get; set; }
}
