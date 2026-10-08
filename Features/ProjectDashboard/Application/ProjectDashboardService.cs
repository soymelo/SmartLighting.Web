using SmartLighting.Web.Features.ProjectDashboard.Contracts.Dtos;
using SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;
using SmartLighting.Web.Infrastructure;

namespace SmartLighting.Web.Features.ProjectDashboard.Application;

public class ProjectDashboardService(IApiClient apiClient) : ServiceBase
{
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
                        zone.Luminaires.Count))
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
                dashboard.RecentSensorEvents
                    .Select(sensorEvent => new ProjectSensorEventVMList(
                        sensorEvent.EventIdentifier,
                        sensorEvent.SensorName,
                        sensorEvent.ZoneName,
                        deviceNames.GetValueOrDefault(sensorEvent.DeviceId, "Controlador não identificado"),
                        sensorEvent.OccurredAtUtc))
                    .ToArray());
        });
    }
}
