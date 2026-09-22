using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.Devices.Application;
using SmartLighting.Web.Features.Devices.Contracts.Dtos;

namespace SmartLighting.Web.Features.Devices.Pages;

public partial class Index
{
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private DeviceService DeviceService { get; set; } = default!;

    private bool isLoading = true;
    private string? errorMessage;
    private IReadOnlyCollection<DeviceDto> devices = [];

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

            if (!result.IsSuccess)
            {
                errorMessage = result.ErrorMessage ?? "Não foi possível carregar os dispositivos.";
                return;
            }

            devices = result.Data?.Devices ?? [];
        }
        finally
        {
            isLoading = false;
        }
    }

    private void OpenDashboard(Guid deviceId)
    {
        Nav.NavigateTo($"/devices/{deviceId}");
    }

}
