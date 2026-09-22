namespace SmartLighting.Web.Features.Devices.Dashboard.Contracts.Dtos;

public sealed record GetDeviceLuminaireStatesDto(
    Guid DeviceId,
    IReadOnlyCollection<LuminaireStateDto> Luminaires);

public sealed record LuminaireStateDto(
    Guid LuminaireId,
    int? BrightnessPercentage,
    DateTime? UpdatedAtUtc);
