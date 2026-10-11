namespace SmartLighting.Web.Features.ProjectDashboard.Contracts.Dtos;

public sealed record GetProjectSensorEventsPageDto(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<GetProjectSensorEventDto> Items);

public sealed record GetProjectSensorEventDto(
    Guid EventIdentifier,
    Guid SensorId,
    Guid DeviceId,
    string SensorName,
    Guid ZoneId,
    string ZoneName,
    DateTime OccurredAtUtc);
