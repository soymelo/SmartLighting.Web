using SmartLighting.Web.Features.Dashboard.Contracts.Dtos;
using SmartLighting.Web.Features.Dashboard.Contracts.ViewModels;
using SmartLighting.Web.Infrastructure;

namespace SmartLighting.Web.Features.Dashboard.Application;

public class DashboardService(IApiClient apiClient) : ServiceBase
{
    public async Task<ServiceResult<DashboardVMList>> GetDashboardAsync(Guid deviceId)
    {
        var endpoint = $"/devices/{deviceId}/dashboard";
        var response = await apiClient.GetAsync<GetDeviceDashboardDto>(endpoint);

        return FromApiResponse(
            response,
            dashboard => new DashboardVMList(
                dashboard.DeviceId,
                dashboard.Name,
                dashboard.LastTelemetryAtUtc,
                dashboard.Luminaires
                    .Select(luminaire => new LuminaireStateVMList(
                        luminaire.LuminaireId,
                        luminaire.Name,
                        luminaire.ZoneName,
                        luminaire.BrightnessPercentage,
                        luminaire.UpdatedAtUtc))
                    .ToArray(),
                dashboard.RecentSensorEvents
                    .Select(sensorEvent => new SensorEventVMList(
                        sensorEvent.EventIdentifier,
                        sensorEvent.SensorId,
                        sensorEvent.SensorName,
                        sensorEvent.ZoneName,
                        sensorEvent.OccurredAtUtc))
                    .ToArray()));
    }
}
