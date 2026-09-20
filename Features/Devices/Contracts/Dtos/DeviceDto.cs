namespace SmartLighting.Web.Features.Devices.Contracts.Dtos;

public sealed record GetDevicesDto(
    IReadOnlyCollection<DeviceDto> Devices);

public sealed record DeviceDto(
    Guid DeviceId,
    string Name,
    DateTime? LastTelemetryAtUtc);