using Microsoft.AspNetCore.Components;
using Radzen;
using SmartLighting.Web.Features.ProjectDashboard.Application;
using SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

namespace SmartLighting.Web.Features.ProjectDashboard.Components;

public partial class ProjectSensorEventsTable
{
    [Parameter, EditorRequired]
    public Guid ProjectId { get; set; }

    [Parameter, EditorRequired]
    public required IReadOnlyCollection<ProjectDeviceVMList> Devices { get; set; }

    [Parameter]
    public EventCallback<int?> TotalCountChanged { get; set; }

    [Inject] private ProjectDashboardService ProjectDashboardService { get; set; } = default!;

    private IReadOnlyCollection<ProjectSensorEventVMList> events = [];
    private int totalCount;
    private bool isLoading;
    private bool hasLoadedFirstPage;
    private string? errorMessage;

    protected override async Task OnInitializedAsync()
    {
        // The grid may render before Radzen invokes LoadData for the first time.
        // Fetch the first page explicitly so the initial view never appears empty.
        await LoadData(new LoadDataArgs { Skip = 0, Top = 10 });
        hasLoadedFirstPage = true;
    }

    private async Task LoadData(LoadDataArgs args)
    {
        isLoading = true;
        errorMessage = null;

        try
        {
            var pageSize = args.Top ?? 10;
            var page = (args.Skip ?? 0) / pageSize + 1;
            var sort = args.Sorts?.FirstOrDefault();
            var sortBy = sort?.Property switch
            {
                nameof(ProjectSensorEventVMList.SensorName) => "sensorName",
                nameof(ProjectSensorEventVMList.ZoneName) => "zoneName",
                nameof(ProjectSensorEventVMList.DeviceName) => "deviceName",
                _ => "occurredAt"
            };
            var descending = sort is null || sort.SortOrder == SortOrder.Descending;

            var result = await ProjectDashboardService.GetSensorEventsAsync(
                ProjectId, page, pageSize, sortBy, descending, Devices);

            if (!result.Success || result.Data is null)
            {
                events = [];
                totalCount = 0;
                errorMessage = result.Message ?? "Não foi possível carregar os eventos dos sensores.";
                await TotalCountChanged.InvokeAsync(null);
                return;
            }

            events = result.Data.Items;
            totalCount = result.Data.TotalCount;
            await TotalCountChanged.InvokeAsync(totalCount);
        }
        finally
        {
            isLoading = false;
        }
    }
}
