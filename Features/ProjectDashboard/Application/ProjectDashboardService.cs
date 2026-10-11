using SmartLighting.Web.Features.ProjectDashboard.Contracts.Dtos;
using SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;
using SmartLighting.Web.Infrastructure;

namespace SmartLighting.Web.Features.ProjectDashboard.Application;

public class ProjectDashboardService(IApiClient apiClient) : ServiceBase
{
    public async Task<ServiceResult<ProjectSensorEventsPageVM>> GetSensorEventsAsync(
        Guid projectId, int page, int pageSize, string sortBy, bool descending,
        IReadOnlyCollection<ProjectDeviceVMList> devices)
    {
        var endpoint = $"/projects/{projectId}/sensor-events";
        var parameters = new Dictionary<string, string?>
        {
            ["page"] = page.ToString(),
            ["pageSize"] = pageSize.ToString(),
            ["sortBy"] = sortBy,
            ["descending"] = descending.ToString().ToLowerInvariant()
        };
        var response = await apiClient.GetAsync<GetProjectSensorEventsPageDto>(endpoint, parameters);

        return FromApiResponse(response, result =>
        {
            var deviceNames = devices.ToDictionary(device => device.DeviceId, device => device.Name);
            return new ProjectSensorEventsPageVM(
                result.TotalCount,
                result.Items.Select(item => new ProjectSensorEventVMList(
                    item.EventIdentifier,
                    item.SensorId,
                    item.DeviceId,
                    item.SensorName,
                    item.ZoneName,
                    deviceNames.GetValueOrDefault(item.DeviceId, "Controlador não identificado"),
                    item.OccurredAtUtc)).ToArray());
        });
    }

    public async Task<ServiceResult<ProjectDashboardVMList>> GetProjectDashboardAsync(Guid projectId)
    {
        var endpoint = $"/projects/{projectId}/dashboard";
        var response = await apiClient.GetAsync<GetProjectDashboardDto>(endpoint);

        return FromApiResponse(response, dashboard =>
        {
            var deviceNames = dashboard.Devices.ToDictionary(device => device.DeviceId, device => device.Name);

            return new ProjectDashboardVMList(
                dashboard.ProjectId,
                dashboard.Name,
                dashboard.Devices
                    .Select(device => new ProjectDeviceVMList(
                        device.DeviceId,
                        device.Name,
                        device.LastTelemetryAtUtc))
                    .ToArray(),
                dashboard.Zones
                    .Select(zone => new ProjectZoneVMList(
                        zone.ZoneId,
                        zone.Name,
                        zone.DisplayOrder,
                        zone.Sensors.Count,
                        zone.Luminaires.Count,
                        zone.Sensors.Select(sensor => new ProjectSensorVMList(
                            sensor.SensorId,
                            sensor.DeviceId,
                            sensor.Name)).ToArray(),
                        zone.Luminaires.Select(luminaire => luminaire.LuminaireId).ToArray()))
                    .ToArray(),
                dashboard.Zones
                    .SelectMany(zone => zone.Luminaires.Select(luminaire => (ZoneName: zone.Name, Luminaire: luminaire)))
                    .GroupBy(item => item.Luminaire.LuminaireId)
                    .Select(group =>
                    {
                        var luminaire = group.First().Luminaire;
                        return new ProjectLuminaireVMList(
                            group.Key,
                            luminaire.Name,
                            string.Join(", ", group.Select(item => item.ZoneName)),
                            deviceNames.GetValueOrDefault(luminaire.DeviceId, "Controlador não identificado"),
                            luminaire.BrightnessPercentage,
                            luminaire.UpdatedAtUtc);
                    })
                    .ToArray(),
                dashboard.ZoneActivity24h
                    .Select(item => new ProjectZoneActivityVMList(item.ZoneId, item.EventCount))
                    .ToArray());
        });
    }
}
