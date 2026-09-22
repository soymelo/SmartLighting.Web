namespace SmartLighting.Web.Features.Dashboard.Contracts.ViewModels;

public sealed record LuminaireStateVMList(
    Guid LuminaireId,
    string LuminaireName,
    string ZoneName,
    int? BrightnessPercentage,
    DateTime? UpdatedAtUtc);
