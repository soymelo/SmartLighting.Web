namespace SmartLighting.Web.Features.Devices.Contracts.ViewModels;

public sealed record DeviceVMList(
    Guid DeviceId,
    string Name,
    DateTime? LastTelemetryAtUtc);
