namespace SmartLighting.Web.Features.ProjectDashboard.Contracts.Dtos;

public sealed record GetProjectDashboardDto(
    Guid ProjectId,
    string Name,
    IReadOnlyCollection<GetProjectDashboardDeviceDto> Devices,
    IReadOnlyCollection<GetProjectDashboardZoneDto> Zones,
    IReadOnlyCollection<GetProjectDashboardSensorEventDto> RecentSensorEvents);

public sealed record GetProjectDashboardDeviceDto(Guid DeviceId, string Name, DateTime? LastTelemetryAtUtc);

public sealed record GetProjectDashboardZoneDto(
    Guid ZoneId,
    string Name,
    int DisplayOrder,
    IReadOnlyCollection<GetProjectDashboardSensorDto> Sensors,
    IReadOnlyCollection<GetProjectDashboardLuminaireDto> Luminaires);

public sealed record GetProjectDashboardSensorDto(Guid SensorId, Guid DeviceId, string Name);

public sealed record GetProjectDashboardLuminaireDto(
    Guid LuminaireId,
    Guid DeviceId,
    string Name,
    decimal NominalPowerWatts,
    int? BrightnessPercentage,
    DateTime? UpdatedAtUtc);

public sealed record GetProjectDashboardSensorEventDto(
    Guid EventIdentifier,
    Guid SensorId,
    Guid DeviceId,
    string SensorName,
    Guid ZoneId,
    string ZoneName,
    DateTime OccurredAtUtc);
