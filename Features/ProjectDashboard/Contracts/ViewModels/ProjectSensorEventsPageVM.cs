namespace SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

public sealed record ProjectSensorEventsPageVM(
    int TotalCount,
    IReadOnlyCollection<ProjectSensorEventVMList> Items);
