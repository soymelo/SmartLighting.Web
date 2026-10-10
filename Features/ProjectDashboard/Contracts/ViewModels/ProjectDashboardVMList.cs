namespace SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

public sealed record ProjectDashboardVMList(
    Guid ProjectId,
    string Name,
    IReadOnlyCollection<ProjectDeviceVMList> Devices,
    IReadOnlyCollection<ProjectZoneVMList> Zones,
    IReadOnlyCollection<ProjectLuminaireVMList> Luminaires,
    IReadOnlyCollection<ProjectSensorEventVMList> RecentSensorEvents,
    IReadOnlyCollection<ProjectZoneActivityVMList> ZoneActivity24h);

public sealed record ProjectZoneActivityVMList(Guid ZoneId, int EventCount);
