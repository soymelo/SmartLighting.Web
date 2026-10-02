using SmartLighting.Web.Features.DeviceDashboard.Contracts.Dtos;
using SmartLighting.Web.Features.DeviceDashboard.Contracts.ViewModels;
using SmartLighting.Web.Infrastructure;

namespace SmartLighting.Web.Features.DeviceDashboard.Application;

public class DeviceDashboardService(IApiClient apiClient) : ServiceBase
{
    public async Task<ServiceResult<DeviceDashboardVMList>> GetDeviceDashboardAsync(Guid deviceId)
    {
        var endpoint = $"/devices/{deviceId}/dashboard";

        var response = await apiClient.GetAsync<GetDeviceDashboardDto>(endpoint);

        return FromApiResponse(
            response,
            dashboard => new DeviceDashboardVMList(
                dashboard.DeviceId,
                dashboard.Name,
                dashboard.LastTelemetryAtUtc,
                dashboard.Luminaires
                    .Select(luminaire => new DeviceLuminaireStateVMList(
                        luminaire.LuminaireId,
                        luminaire.Name,
                        luminaire.ZoneName,
                        luminaire.BrightnessPercentage,
                        luminaire.UpdatedAtUtc))
                    .ToArray(),
                dashboard.RecentSensorEvents
                    .Select(sensorEvent => new DeviceSensorEventVMList(
                        sensorEvent.EventIdentifier,
                        sensorEvent.SensorId,
                        sensorEvent.SensorName,
                        sensorEvent.ZoneName,
                        sensorEvent.OccurredAtUtc))
                    .ToArray()));
    }
}
