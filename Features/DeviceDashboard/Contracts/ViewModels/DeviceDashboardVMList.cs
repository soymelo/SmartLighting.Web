namespace SmartLighting.Web.Features.DeviceDashboard.Contracts.ViewModels;

public sealed record DeviceDashboardVMList(
    Guid DeviceId,
    string Name,
    DateTime? LastTelemetryAtUtc,
    IReadOnlyCollection<DeviceLuminaireStateVMList> Luminaires,
    IReadOnlyCollection<DeviceSensorEventVMList> RecentSensorEvents);
