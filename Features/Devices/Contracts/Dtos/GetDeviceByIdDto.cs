namespace SmartLighting.Web.Features.Devices.Contracts.Dtos;

public sealed record GetDeviceByIdDto(
    Guid DeviceId,
    string Name,
    Guid ProjectId,
    string ProjectName,
    DateTime? LastTelemetryAtUtc,
    IReadOnlyCollection<GetDeviceZoneDto> Zones);

public sealed record GetDeviceZoneDto(
    Guid ZoneId,
    string Name,
    int DisplayOrder,
    IReadOnlyCollection<GetDeviceSensorDto> Sensors,
    IReadOnlyCollection<GetDeviceLuminaireDto> Luminaires);

public sealed record GetDeviceSensorDto(Guid SensorId, string Name);

public sealed record GetDeviceLuminaireDto(Guid LuminaireId, string Name, decimal NominalPowerWatts);
