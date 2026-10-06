namespace SmartLighting.Web.Features.Devices.Contracts.Dtos;

public sealed record UpdateDeviceDto(
    string Name,
    IReadOnlyCollection<UpdateDeviceSensorDto> Sensors,
    IReadOnlyCollection<UpdateDeviceLuminaireDto> Luminaires);

public sealed record UpdateDeviceSensorDto(int Slot, string Name, Guid ZoneId);

public sealed record UpdateDeviceLuminaireDto(
    int Slot,
    string Name,
    Guid ZoneId,
    decimal NominalPowerWatts);

public sealed record UpdateDeviceResponseDto(
    Guid DeviceId,
    string Name,
    int SensorCount,
    int LuminaireCount);
