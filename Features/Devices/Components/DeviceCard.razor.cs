using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.Devices.Contracts.ViewModels;

namespace SmartLighting.Web.Features.Devices.Components;

public partial class DeviceCard
{
    private static readonly TimeZoneInfo BrasiliaTimeZone =
    TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

    [Parameter, EditorRequired]
    public required DeviceVMList Device { get; set; }

    [Parameter]
    public EventCallback<Guid> OnOpenDeviceDashboard { get; set; }

    private static string FormatLastTelemetry(DateTime? lastTelemetryAtUtc)
    {
        if (lastTelemetryAtUtc is null)
        {
            return "Nenhuma telemetria recebida";
        }

        var telemetryAtUtc = DateTime.SpecifyKind(lastTelemetryAtUtc.Value, DateTimeKind.Utc);
        var telemetryAtBrasilia = TimeZoneInfo.ConvertTimeFromUtc(telemetryAtUtc, BrasiliaTimeZone);

        return $"{telemetryAtBrasilia:dd/MM/yyyy 'às' HH:mm}";
    }
}
