using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SmartLighting.Web.Features.Devices.Application;
using SmartLighting.Web.Features.Devices.Contracts.Dtos;
using SmartLighting.Web.Features.Devices.Contracts.ViewModels;

namespace SmartLighting.Web.Features.Devices.Pages;

public partial class EditDevice
{
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private DeviceService DeviceService { get; set; } = default!;

    [Parameter] public Guid DeviceId { get; set; }

    private CreateDeviceFormVM form = new();
    private readonly Dictionary<string, string> validationErrors = [];
    private DeviceVM? device;
    private DeviceSetupVM? deviceSetup;
    private bool isLoading = true;
    private bool isSubmitting;
    private bool isNotFound;
    private string? loadErrorMessage;
    private string? submitErrorMessage;

    protected override async Task OnParametersSetAsync()
    {
        isLoading = true;
        isNotFound = false;
        loadErrorMessage = null;
        submitErrorMessage = null;
        device = null;
        deviceSetup = null;
        validationErrors.Clear();

        try
        {
            var deviceResult = await DeviceService.GetDeviceByIdAsync(DeviceId);
            if (!deviceResult.Success)
            {
                isNotFound = deviceResult.StatusCode == HttpStatusCode.NotFound;
                loadErrorMessage = isNotFound
                    ? null
                    : deviceResult.Message ?? "Não foi possível carregar o dispositivo.";
                return;
            }

            device = deviceResult.Data;
            if (device is null)
            {
                loadErrorMessage = "A API não retornou os dados do dispositivo.";
                return;
            }

            var setupResult = await DeviceService.GetDeviceSetupAsync(device.ProjectId);
            if (!setupResult.Success || setupResult.Data is null)
            {
                loadErrorMessage = setupResult.Message ?? "Não foi possível carregar as zonas do projeto.";
                return;
            }

            deviceSetup = setupResult.Data;
            PopulateForm(device);
        }
        finally
        {
            isLoading = false;
        }
    }

    private void PopulateForm(DeviceVM currentDevice)
    {
        form = new CreateDeviceFormVM { Name = currentDevice.Name };

        foreach (var zone in currentDevice.Zones)
        {
            foreach (var sensor in zone.Sensors)
            {
                var slot = form.Sensors.Single(item => item.Slot == sensor.Slot);
                slot.Enabled = true;
                slot.IsPersisted = true;
                slot.Name = sensor.Name;
                slot.ZoneId = zone.ZoneId;
            }

            foreach (var luminaire in zone.Luminaires)
            {
                var slot = form.Luminaires.Single(item => item.Slot == luminaire.Slot);
                slot.Enabled = true;
                slot.IsPersisted = true;
                slot.Name = luminaire.Name;
                slot.ZoneId = zone.ZoneId;
                slot.NominalPowerWatts = luminaire.NominalPowerWatts;
            }
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
            submitErrorMessage = "Revise os campos destacados antes de salvar o dispositivo.";
            return;
        }

        isSubmitting = true;
        try
        {
            var request = new UpdateDeviceDto(
                form.Name.Trim(),
                form.Sensors.Where(sensor => sensor.Enabled)
                    .Select(sensor => new UpdateDeviceSensorDto(
                        sensor.Slot, sensor.Name.Trim(), sensor.ZoneId!.Value)).ToArray(),
                form.Luminaires.Where(luminaire => luminaire.Enabled)
                    .Select(luminaire => new UpdateDeviceLuminaireDto(
                        luminaire.Slot, luminaire.Name.Trim(), luminaire.ZoneId!.Value,
                        luminaire.NominalPowerWatts!.Value)).ToArray());

            var result = await DeviceService.UpdateDeviceAsync(DeviceId, request);
            if (!result.Success)
            {
                submitErrorMessage = result.Message ?? "Não foi possível atualizar o dispositivo.";
                return;
            }

            Nav.NavigateTo($"/devices/{DeviceId}?updated=true");
        }
        finally
        {
            isSubmitting = false;
        }
    }

    private void NavigateToDevice() => Nav.NavigateTo($"/devices/{DeviceId}");
    private void NavigateToProjects() => Nav.NavigateTo("/projects");
}
