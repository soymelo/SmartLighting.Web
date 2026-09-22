namespace SmartLighting.Web.Features.Dashboard.Contracts.ViewModels;

public sealed record TelemetryStatusVMList(
    Guid DeviceId,
    DateTime? LastTelemetryAtUtc);
