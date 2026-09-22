namespace SmartLighting.Web.Features.Devices.Dashboard.Contracts.Dtos;

public sealed record GetDeviceTelemetryStatusDto(
    Guid DeviceId,
    DateTime? LastTelemetryAtUtc);
