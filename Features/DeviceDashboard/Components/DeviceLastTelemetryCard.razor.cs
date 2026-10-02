using Microsoft.AspNetCore.Components;

namespace SmartLighting.Web.Features.DeviceDashboard.Components;

public partial class DeviceLastTelemetryCard
{
    [Parameter]
    public DateTime? LastTelemetryAtUtc { get; set; }

    private string LastTelemetryText => LastTelemetryAtUtc is null
        ? "Nenhuma telemetria recebida"
        : DeviceDashboardDateFormatter.Format(LastTelemetryAtUtc.Value);
}
