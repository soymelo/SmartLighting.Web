namespace SmartLighting.Web.Features.DeviceDashboard.Contracts.ViewModels;

public sealed record DeviceLuminaireStateVMList(
    Guid LuminaireId,
    string LuminaireName,
    string ZoneName,
    int? BrightnessPercentage,
    DateTime? UpdatedAtUtc);
