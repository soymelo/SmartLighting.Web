using System.Net;
using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.DeviceDashboard.Application;
using SmartLighting.Web.Features.DeviceDashboard.Contracts.ViewModels;

namespace SmartLighting.Web.Features.DeviceDashboard.Pages;

public partial class Index
{
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private DeviceDashboardService DeviceDashboardService { get; set; } = default!;

    [Parameter] public Guid DeviceId { get; set; }

    private bool isLoading = true;
    private bool isNotFound;
    private string? errorMessage;
    private DeviceDashboardVMList? deviceDashboardVM;

    private int luminairesWithStateCount =>
        deviceDashboardVM?.Luminaires.Count(luminaire => luminaire.BrightnessPercentage is not null) ?? 0;

    protected override async Task OnParametersSetAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        isLoading = true;
        isNotFound = false;
        errorMessage = null;
        deviceDashboardVM = null;

        try
        {
            var result = await DeviceDashboardService.GetDeviceDashboardAsync(DeviceId);

            if (!result.Success)
            {
                isNotFound = result.StatusCode == HttpStatusCode.NotFound;
                errorMessage = isNotFound
                    ? null
                    : result.Message ?? "Não foi possível carregar o dashboard.";
                return;
            }

            deviceDashboardVM = result.Data;
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
