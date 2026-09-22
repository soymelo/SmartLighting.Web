using SmartLighting.Web.Features.Devices.Dashboard.Contracts.Dtos;
using SmartLighting.Web.Infrastructure;

namespace SmartLighting.Web.Features.Devices.Dashboard.Application;

public class DashboardService(IApiClient apiClient)
{
    public async Task<DashboardResult> GetDashboardAsync(Guid deviceId)
    {
        var topologyTask = GetDeviceTopologyAsync(deviceId);
        var telemetryStatusTask = GetDeviceTelemetryStatusAsync(deviceId);
        var luminaireStatesTask = GetDeviceLuminaireStatesAsync(deviceId);
        var recentSensorEventsTask = GetRecentSensorEventsAsync(deviceId);

        await Task.WhenAll(
            topologyTask,
            telemetryStatusTask,
            luminaireStatesTask,
            recentSensorEventsTask);

        return new DashboardResult(
            await topologyTask,
            await telemetryStatusTask,
            await luminaireStatesTask,
            await recentSensorEventsTask);
    }

    public Task<ApiResponse<GetDeviceTopologyDto>> GetDeviceTopologyAsync(Guid deviceId)
    {
        var endpoint = $"/devices/{deviceId}/topology";

        return apiClient.GetAsync<GetDeviceTopologyDto>(endpoint);
    }

    public Task<ApiResponse<GetDeviceTelemetryStatusDto>> GetDeviceTelemetryStatusAsync(Guid deviceId)
    {
        var endpoint = $"/devices/{deviceId}/telemetry/status";

        return apiClient.GetAsync<GetDeviceTelemetryStatusDto>(endpoint);
    }

    public Task<ApiResponse<GetDeviceLuminaireStatesDto>> GetDeviceLuminaireStatesAsync(Guid deviceId)
    {
        var endpoint = $"/devices/{deviceId}/telemetry/luminaire-states";

        return apiClient.GetAsync<GetDeviceLuminaireStatesDto>(endpoint);
    }

    public Task<ApiResponse<GetRecentSensorEventsDto>> GetRecentSensorEventsAsync(Guid deviceId, int limit = 50)
    {
        var endpoint = $"/devices/{deviceId}/telemetry/sensor-events";

        var parameters = new Dictionary<string, string?>
        {
            ["limit"] = limit.ToString()
        };

        return apiClient.GetAsync<GetRecentSensorEventsDto>(
            endpoint,
            parameters);
    }
}
