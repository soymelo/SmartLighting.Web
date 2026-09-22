namespace SmartLighting.Web.Features.Devices.Dashboard.Contracts.Dtos;

public sealed record GetDeviceTopologyDto(
    Guid DeviceId,
    string Name,
    IReadOnlyCollection<ZoneDto> Zones);

public sealed record ZoneDto(
    Guid ZoneId,
    string Name,
    int DisplayOrder,
    IReadOnlyCollection<SensorDto> Sensors,
    IReadOnlyCollection<LuminaireDto> Luminaires);

public sealed record SensorDto(Guid SensorId, string Name);

public sealed record LuminaireDto(
    Guid LuminaireId,
    string Name,
    decimal NominalPowerWatts);
