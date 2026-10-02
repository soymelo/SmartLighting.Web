namespace SmartLighting.Web.Features.DeviceDashboard.Contracts.Dtos;

public sealed record GetDeviceDashboardDto(
    Guid DeviceId,
    string Name,
    DateTime? LastTelemetryAtUtc,
    IReadOnlyCollection<GetDeviceDashboardLuminaireDto> Luminaires,
    IReadOnlyCollection<GetDeviceDashboardSensorEventDto> RecentSensorEvents);

public sealed record GetDeviceDashboardLuminaireDto(
    Guid LuminaireId,
    string Name,
    string ZoneName,
    int? BrightnessPercentage,
    DateTime? UpdatedAtUtc);

public sealed record GetDeviceDashboardSensorEventDto(
    Guid EventIdentifier,
    Guid SensorId,
    string SensorName,
    string ZoneName,
    DateTime OccurredAtUtc);
