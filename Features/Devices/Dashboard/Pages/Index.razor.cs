using System.Net;
using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.Devices.Dashboard.Application;
using SmartLighting.Web.Features.Devices.Dashboard.Contracts.Dtos;

namespace SmartLighting.Web.Features.Devices.Dashboard.Pages;

public partial class Index
{
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private DashboardService DashboardService { get; set; } = default!;

    [Parameter] public Guid DeviceId { get; set; }
    
    //estado geral da página
    private bool isLoading = true;
    private DashboardResult? dashboard;

    private GetDeviceTopologyDto? topology => dashboard?.Topology.Data;

    private bool isNotFound => dashboard?.Topology.StatusCode == HttpStatusCode.NotFound;

    private string? dashboardError => dashboard is null ||
        (dashboard.Topology.IsSuccess && dashboard.Topology.Data is not null)
            ? null
            : dashboard.Topology.ErrorMessage ?? "Não foi possível carregar o dashboard.";

    protected override async Task OnParametersSetAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        isLoading = true;
        dashboard = null;

        try
        {
            dashboard = await DashboardService.GetDashboardAsync(DeviceId);
        }
        finally
        {
            isLoading = false;
        }
    }

    private void NavigateToDevices()
    {
        Nav.NavigateTo("/devices");
    }
}
