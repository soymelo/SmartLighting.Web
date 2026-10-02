using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.Devices.Application;
using SmartLighting.Web.Features.Devices.Contracts.ViewModels;

namespace SmartLighting.Web.Features.Devices.Pages;

public partial class Index
{
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private DeviceService DeviceService { get; set; } = default!;

    private bool isLoading = true;
    private string? errorMessage;
    private IReadOnlyCollection<DeviceVMList> devices = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        isLoading = true;
        errorMessage = null;

        try
        {
            var result = await DeviceService.GetDevicesAsync();

            if (!result.Success)
            {
                errorMessage = result.Message ?? "Não foi possível carregar os dispositivos.";
                return;
            }

            devices = result.Data ?? [];
        }
        finally
        {
            isLoading = false;
        }
    }

    private void OpenDeviceDashboard(Guid deviceId)
    {
        Nav.NavigateTo($"/devices/{deviceId}");
    }

}
