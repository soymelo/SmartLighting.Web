namespace SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

public sealed record ProjectSensorEventVMList(
    Guid EventIdentifier,
    string SensorName,
    string ZoneName,
    string DeviceName,
    DateTime OccurredAtUtc);
