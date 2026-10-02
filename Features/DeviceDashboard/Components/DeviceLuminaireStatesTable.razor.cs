using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.DeviceDashboard.Contracts.ViewModels;

namespace SmartLighting.Web.Features.DeviceDashboard.Components;

public partial class DeviceLuminaireStatesTable
{
    [Parameter, EditorRequired]
    public required IReadOnlyCollection<DeviceLuminaireStateVMList> Luminaires { get; set; }
}
