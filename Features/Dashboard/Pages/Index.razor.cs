using System.Net;
using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.Dashboard.Application;
using SmartLighting.Web.Features.Dashboard.Contracts.ViewModels;

namespace SmartLighting.Web.Features.Dashboard.Pages;

public partial class Index
{
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private DashboardService DashboardService { get; set; } = default!;

    [Parameter] public Guid DeviceId { get; set; }

    private bool isLoading = true;
    private bool isNotFound;
    private string? errorMessage;
    private DashboardVMList? dashboardVM;

    protected override async Task OnParametersSetAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        isLoading = true;
        isNotFound = false;
        errorMessage = null;
        dashboardVM = null;

        try
        {
            var result = await DashboardService.GetDashboardAsync(DeviceId);

            if (!result.Success)
            {
                isNotFound = result.StatusCode == HttpStatusCode.NotFound;
                errorMessage = isNotFound
                    ? null
                    : result.Message ?? "Não foi possível carregar o dashboard.";
                return;
            }

            dashboardVM = result.Data;
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
