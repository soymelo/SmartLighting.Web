namespace SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

public sealed record ProjectLuminaireVMList(
    Guid LuminaireId,
    string Name,
    string ZoneName,
    string DeviceName,
    int? BrightnessPercentage,
    DateTime? UpdatedAtUtc);
