namespace SmartLighting.Web.Features.Devices.Contracts.Dtos;

public sealed record CreateDeviceDto(
    string Name,
    IReadOnlyCollection<CreateDeviceSensorDto> Sensors,
    IReadOnlyCollection<CreateDeviceLuminaireDto> Luminaires);

public sealed record CreateDeviceSensorDto(
    int Slot,
    string Name,
    Guid ZoneId);

public sealed record CreateDeviceLuminaireDto(
    int Slot,
    string Name,
    Guid ZoneId,
    decimal NominalPowerWatts);

public sealed record CreateDeviceResponseDto(
    Guid DeviceId,
    string Name,
    int SensorsCreated,
    int LuminairesCreated);
