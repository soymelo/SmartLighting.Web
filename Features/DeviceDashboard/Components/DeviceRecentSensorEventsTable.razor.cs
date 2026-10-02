using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.DeviceDashboard.Contracts.ViewModels;

namespace SmartLighting.Web.Features.DeviceDashboard.Components;

public partial class DeviceRecentSensorEventsTable
{
    [Parameter, EditorRequired]
    public required IReadOnlyCollection<DeviceSensorEventVMList> Events { get; set; }
}
