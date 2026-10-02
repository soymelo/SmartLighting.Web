namespace SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

public sealed record ProjectDeviceVMList(Guid DeviceId, string Name, DateTime? LastTelemetryAtUtc);
