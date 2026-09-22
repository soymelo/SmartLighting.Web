using Microsoft.AspNetCore.Components;

namespace SmartLighting.Web.Features.Devices.Dashboard.Components;

public partial class LastTelemetryCard
{
    [Parameter]
    public DateTime? LastTelemetryAtUtc { get; set; }

    private string LastTelemetryText => LastTelemetryAtUtc is null
        ? "Nenhuma telemetria recebida"
        : $"{DashboardDateFormatter.Format(LastTelemetryAtUtc.Value)} (horário de Brasília)";
}
