namespace SmartLighting.Web.Features.Dashboard.Contracts.Dtos;

public sealed record GetDeviceTelemetryStatusDto(
    Guid DeviceId,
    DateTime? LastTelemetryAtUtc);
