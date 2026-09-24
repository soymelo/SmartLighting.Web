using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.Dashboard.Contracts.ViewModels;

namespace SmartLighting.Web.Features.Dashboard.Components;

public partial class RecentSensorEventsTable
{
    [Parameter, EditorRequired]
    public required IReadOnlyCollection<SensorEventVMList> Events { get; set; }
}
