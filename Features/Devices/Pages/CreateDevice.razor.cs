using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SmartLighting.Web.Features.Devices.Application;
using SmartLighting.Web.Features.Devices.Contracts.Dtos;
using SmartLighting.Web.Features.Devices.Contracts.ViewModels;

namespace SmartLighting.Web.Features.Devices.Pages;

public partial class CreateDevice
{
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private DeviceService DeviceService { get; set; } = default!;

    [Parameter] public Guid ProjectId { get; set; }

    private readonly CreateDeviceFormVM form = new();
    private readonly Dictionary<string, string> validationErrors = [];
    private DeviceSetupVM? deviceSetup;
    private bool isLoading = true;
    private bool isSubmitting;
    private bool isNotFound;
    private string? loadErrorMessage;
    private string? submitErrorMessage;

    protected override async Task OnParametersSetAsync()
    {
        await LoadProjectAsync();
    }

    private async Task LoadProjectAsync()
    {
        isLoading = true;
        isNotFound = false;
        loadErrorMessage = null;
        deviceSetup = null;

        try
        {
            var result = await DeviceService.GetDeviceSetupAsync(ProjectId);

            if (!result.Success)
            {
                isNotFound = result.StatusCode == HttpStatusCode.NotFound;
                loadErrorMessage = isNotFound
                    ? null
                    : result.Message ?? "Não foi possível carregar os dados do projeto.";
                return;
            }

            deviceSetup = result.Data;

            if (deviceSetup is null)
            {
                loadErrorMessage = "A API não retornou os dados do projeto.";
            }
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task HandleSubmit(EditContext _)
    {
        if (isSubmitting || deviceSetup is null)
        {
            return;
        }

        submitErrorMessage = null;

        if (!DeviceFormValidator.Validate(form, validationErrors))
        {
            submitErrorMessage = "Revise os campos destacados antes de cadastrar o dispositivo.";
            return;
        }

        isSubmitting = true;
        submitErrorMessage = null;

        try
        {
            var request = new CreateDeviceDto(
                form.Name.Trim(),
                form.Sensors
                    .Where(sensor => sensor.Enabled)
                    .Select(sensor => new CreateDeviceSensorDto(
                        sensor.Slot,
                        sensor.Name.Trim(),
                        sensor.ZoneId!.Value))
                    .ToArray(),
                form.Luminaires
                    .Where(luminaire => luminaire.Enabled)
                    .Select(luminaire => new CreateDeviceLuminaireDto(
                        luminaire.Slot,
                        luminaire.Name.Trim(),
                        luminaire.ZoneId!.Value,
                        luminaire.NominalPowerWatts!.Value))
                    .ToArray());

            var result = await DeviceService.CreateDeviceAsync(ProjectId, request);

            if (!result.Success)
            {
                submitErrorMessage = result.Message ?? "Não foi possível cadastrar o dispositivo.";
                return;
            }

            if (result.Data is null)
            {
                submitErrorMessage = "A API não retornou o dispositivo cadastrado.";
                return;
            }

            Nav.NavigateTo($"/devices/{result.Data.DeviceId}?created=true");
        }
        finally
        {
            isSubmitting = false;
        }
    }


    private void NavigateToDashboard() =>
        Nav.NavigateTo($"/projects/{ProjectId}/dashboard");

    private void NavigateToProjects() =>
        Nav.NavigateTo("/projects");
}
