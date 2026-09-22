namespace SmartLighting.Web.Features.Dashboard.Contracts.ViewModels;

public sealed record SensorEventVMList(
    Guid EventIdentifier,
    Guid SensorId,
    string SensorName,
    string ZoneName,
    DateTime OccurredAtUtc);
