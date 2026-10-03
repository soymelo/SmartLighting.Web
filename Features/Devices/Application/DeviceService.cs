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
                        .Select(sensor => new DeviceSensorVM(sensor.SensorId, sensor.Name))
                        .ToArray(),
                    zone.Luminaires
                        .Select(luminaire => new DeviceLuminaireVM(
                            luminaire.LuminaireId,
                            luminaire.Name,
                            luminaire.NominalPowerWatts))
                        .ToArray()))
                .ToArray()));
    }
}
