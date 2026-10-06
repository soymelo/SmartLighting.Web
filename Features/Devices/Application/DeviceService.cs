using SmartLighting.Web.Features.Devices.Contracts.Dtos;
using SmartLighting.Web.Features.Devices.Contracts.ViewModels;
using SmartLighting.Web.Infrastructure;

namespace SmartLighting.Web.Features.Devices.Application;

public class DeviceService(IApiClient apiClient) : ServiceBase
{
    public async Task<ServiceResult<DeviceVM>> GetDeviceByIdAsync(Guid deviceId)
    {
        var response = await apiClient.GetAsync<GetDeviceByIdDto>($"/devices/{deviceId}");

        return FromApiResponse(response, device => new DeviceVM(
            device.DeviceId,
            device.Name,
            device.ProjectId,
            device.ProjectName,
            device.LastTelemetryAtUtc,
            device.Zones
                .Select(zone => new DeviceZoneVM(
                    zone.ZoneId,
                    zone.Name,
                    zone.DisplayOrder,
                    zone.Sensors
                        .Select(sensor => new DeviceSensorVM(
                            sensor.SensorId,
                            sensor.Name,
                            sensor.Slot,
                            sensor.GpioNumber))
                        .ToArray(),
                    zone.Luminaires
                        .Select(luminaire => new DeviceLuminaireVM(
                            luminaire.LuminaireId,
                            luminaire.Name,
                            luminaire.NominalPowerWatts,
                            luminaire.Slot,
                            luminaire.GpioNumber))
                        .ToArray()))
                .ToArray()));
    }

    public async Task<ServiceResult<DeviceSetupVM>> GetDeviceSetupAsync(Guid projectId)
    {
        var response = await apiClient.GetAsync<GetDeviceSetupDto>($"/projects/{projectId}/dashboard");

        return FromApiResponse(response, project => new DeviceSetupVM(
            project.ProjectId,
            project.Name,
            project.Zones
                .OrderBy(zone => zone.DisplayOrder)
                .ThenBy(zone => zone.ZoneId)
                .Select(zone => new DeviceSetupZoneVM(
                    zone.ZoneId,
                    zone.Name,
                    zone.DisplayOrder))
                .ToArray()));
    }

    public async Task<ServiceResult<CreateDeviceResponseDto>> CreateDeviceAsync(
        Guid projectId,
        CreateDeviceDto device)
    {
        var response = await apiClient.PostAsync<CreateDeviceDto, CreateDeviceResponseDto>(
            $"/projects/{projectId}/devices",
            device);

        return FromApiResponse(response);
    }

    public async Task<ServiceResult<UpdateDeviceResponseDto>> UpdateDeviceAsync(
        Guid deviceId,
        UpdateDeviceDto device)
    {
        var response = await apiClient.PutAsync<UpdateDeviceDto, UpdateDeviceResponseDto>(
            $"/devices/{deviceId}", device);

        return FromApiResponse(response);
    }
}
