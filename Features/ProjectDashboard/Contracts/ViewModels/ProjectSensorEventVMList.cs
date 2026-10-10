namespace SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

public sealed record ProjectSensorEventVMList(
    Guid EventIdentifier,
    Guid SensorId,
    Guid DeviceId,
    string SensorName,
    string ZoneName,
    string DeviceName,
    DateTime OccurredAtUtc);
