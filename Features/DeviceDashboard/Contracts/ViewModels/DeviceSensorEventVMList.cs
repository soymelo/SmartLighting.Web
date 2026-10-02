namespace SmartLighting.Web.Features.DeviceDashboard.Contracts.ViewModels;

public sealed record DeviceSensorEventVMList(
    Guid EventIdentifier,
    Guid SensorId,
    string SensorName,
    string ZoneName,
    DateTime OccurredAtUtc);
