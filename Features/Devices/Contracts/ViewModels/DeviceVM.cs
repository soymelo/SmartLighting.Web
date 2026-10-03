namespace SmartLighting.Web.Features.Devices.Contracts.ViewModels;

public sealed record DeviceVM(
    Guid DeviceId,
    string Name,
    Guid ProjectId,
    string ProjectName,
    DateTime? LastTelemetryAtUtc,
    IReadOnlyCollection<DeviceZoneVM> Zones);

public sealed record DeviceZoneVM(
    Guid ZoneId,
    string Name,
    int DisplayOrder,
    IReadOnlyCollection<DeviceSensorVM> Sensors,
    IReadOnlyCollection<DeviceLuminaireVM> Luminaires);

public sealed record DeviceSensorVM(
    Guid SensorId,
    string Name,
    int Slot,
    int GpioNumber);

public sealed record DeviceLuminaireVM(
    Guid LuminaireId,
    string Name,
    decimal NominalPowerWatts,
    int Slot,
    int GpioNumber);
