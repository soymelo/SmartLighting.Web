using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

namespace SmartLighting.Web.Features.ProjectDashboard.Components;

public partial class ProjectDevicesTable
{
    [Parameter, EditorRequired]
    public required IReadOnlyCollection<ProjectDeviceVMList> Devices { get; set; }

    [Parameter]
    public EventCallback<Guid> OpenDevice { get; set; }
}
