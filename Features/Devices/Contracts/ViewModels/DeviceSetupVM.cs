namespace SmartLighting.Web.Features.Devices.Contracts.ViewModels;

public sealed record DeviceSetupVM(
    Guid ProjectId,
    string ProjectName,
    IReadOnlyCollection<DeviceSetupZoneVM> Zones);

public sealed record DeviceSetupZoneVM(
    Guid ZoneId,
    string Name,
    int DisplayOrder);
