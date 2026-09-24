namespace SmartLighting.Web.Features.Dashboard.Contracts.ViewModels;

public sealed record DashboardVMList(
    Guid DeviceId,
    string Name,
    DateTime? LastTelemetryAtUtc,
    IReadOnlyCollection<LuminaireStateVMList> Luminaires,
    IReadOnlyCollection<SensorEventVMList> RecentSensorEvents);
