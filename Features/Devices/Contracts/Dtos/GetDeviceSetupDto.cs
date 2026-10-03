namespace SmartLighting.Web.Features.Devices.Contracts.Dtos;

public sealed record GetDeviceSetupDto(
    Guid ProjectId,
    string Name,
    IReadOnlyCollection<GetDeviceSetupZoneDto> Zones);

public sealed record GetDeviceSetupZoneDto(
    Guid ZoneId,
    string Name,
    int DisplayOrder);
