using System.Net;
using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.Devices.Application;
using SmartLighting.Web.Features.Devices.Contracts.ViewModels;

namespace SmartLighting.Web.Features.Devices.Pages;

public partial class Index
{
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private DeviceService DeviceService { get; set; } = default!;

    [Parameter] public Guid DeviceId { get; set; }
    [SupplyParameterFromQuery(Name = "created")]
    public bool Created { get; set; }
    [SupplyParameterFromQuery(Name = "updated")]
    public bool Updated { get; set; }

    private bool isLoading = true;
    private bool isNotFound;
    private string? errorMessage;
    private DeviceVM? device;

    protected override async Task OnParametersSetAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        isLoading = true;
        isNotFound = false;
        errorMessage = null;
        device = null;

        try
        {
            var result = await DeviceService.GetDeviceByIdAsync(DeviceId);

            if (!result.Success)
            {
                isNotFound = result.StatusCode == HttpStatusCode.NotFound;
                errorMessage = isNotFound
                    ? null
                    : result.Message ?? "Não foi possível carregar o dispositivo.";
                return;
            }

            device = result.Data;

            if (device is null)
            {
                errorMessage = "A API não retornou os dados do dispositivo.";
            }
        }
        finally
        {
            isLoading = false;
        }
    }

    private void NavigateBack()
    {
        if (device is not null)
        {
            Nav.NavigateTo($"/projects/{device.ProjectId}/dashboard");
        }
    }

    private void NavigateToProjects()
    {
        Nav.NavigateTo("/projects");
    }

    private void NavigateToEdit() => Nav.NavigateTo($"/devices/{DeviceId}/edit");
}
