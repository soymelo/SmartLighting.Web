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

    private int enabledSensorCount => form.Sensors.Count(sensor => sensor.Enabled);
    private int enabledLuminaireCount => form.Luminaires.Count(luminaire => luminaire.Enabled);
    private int enabledComponentCount => enabledSensorCount + enabledLuminaireCount;

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
        if (isSubmitting || deviceSetup is null || !ValidateForm())
        {
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

    private bool ValidateForm()
    {
        validationErrors.Clear();
        submitErrorMessage = null;

        if (string.IsNullOrWhiteSpace(form.Name))
        {
            validationErrors["name"] = "Informe o nome do dispositivo.";
        }
        else if (form.Name.Trim().Length > 50)
        {
            validationErrors["name"] = "O nome deve ter no máximo 50 caracteres.";
        }

        foreach (var sensor in form.Sensors.Where(sensor => sensor.Enabled))
        {
            ValidateComponentName(sensor.Name, $"sensor-{sensor.Slot}-name");

            if (sensor.ZoneId is null)
            {
                validationErrors[$"sensor-{sensor.Slot}-zone"] = "Selecione a zona do sensor.";
            }
        }

        foreach (var luminaire in form.Luminaires.Where(luminaire => luminaire.Enabled))
        {
            ValidateComponentName(luminaire.Name, $"luminaire-{luminaire.Slot}-name");

            if (luminaire.ZoneId is null)
            {
                validationErrors[$"luminaire-{luminaire.Slot}-zone"] = "Selecione a zona da luminária.";
            }

            if (luminaire.NominalPowerWatts is null or <= 0)
            {
                validationErrors[$"luminaire-{luminaire.Slot}-power"] = "Informe uma potência maior que zero.";
            }
            else if (luminaire.NominalPowerWatts > 99999999.99m ||
                     decimal.Round(luminaire.NominalPowerWatts.Value, 2) != luminaire.NominalPowerWatts.Value)
            {
                validationErrors[$"luminaire-{luminaire.Slot}-power"] = "Use até 8 dígitos inteiros e 2 casas decimais.";
            }
        }

        if (validationErrors.Count > 0)
        {
            submitErrorMessage = "Revise os campos destacados antes de cadastrar o dispositivo.";
        }

        return validationErrors.Count == 0;
    }

    private void ValidateComponentName(string name, string key)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            validationErrors[key] = "Informe o nome do componente.";
        }
        else if (name.Trim().Length > 50)
        {
            validationErrors[key] = "O nome deve ter no máximo 50 caracteres.";
        }
    }

    private string? GetError(string key) => validationErrors.GetValueOrDefault(key);

    private void NavigateToDashboard() =>
        Nav.NavigateTo($"/projects/{ProjectId}/dashboard");

    private void NavigateToProjects() =>
        Nav.NavigateTo("/projects");
}
