namespace SmartLighting.Web.Features.Devices.Dashboard.Contracts.Dtos;

public sealed record GetRecentSensorEventsDto(
    Guid DeviceId,
    IReadOnlyCollection<SensorEventDto> Events);

public sealed record SensorEventDto(
    Guid EventIdentifier,
    Guid SensorId,
    DateTime OccurredAtUtc);
