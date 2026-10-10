namespace SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

public sealed record ProjectZoneVMList(
    Guid ZoneId,
    string Name,
    int DisplayOrder,
    int SensorCount,
    int LuminaireCount,
    IReadOnlyCollection<ProjectSensorVMList> Sensors,
    IReadOnlyCollection<Guid> LuminaireIds);

public sealed record ProjectSensorVMList(Guid SensorId, Guid DeviceId, string Name);
