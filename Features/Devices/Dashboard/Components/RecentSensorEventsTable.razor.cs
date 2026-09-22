using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.Devices.Dashboard.Contracts.Dtos;

namespace SmartLighting.Web.Features.Devices.Dashboard.Components;

public partial class RecentSensorEventsTable
{
    [Parameter, EditorRequired]
    public required GetDeviceTopologyDto Topology { get; set; }

    [Parameter, EditorRequired]
    public required GetRecentSensorEventsDto Events { get; set; }

    private IReadOnlyCollection<SensorEventRow> rows = [];

    protected override void OnParametersSet()
    {
        var sensorsById = Topology.Zones
            .SelectMany(zone => zone.Sensors.Select(sensor => new SensorDetails(sensor.SensorId, sensor.Name, zone.Name)))
            .ToDictionary(sensor => sensor.SensorId);

        rows = Events.Events
            .Select(sensorEvent =>
            {
                sensorsById.TryGetValue(sensorEvent.SensorId, out var sensor);

                return new SensorEventRow(
                    sensorEvent.EventIdentifier,
                    sensor?.Name ?? "Sensor não encontrado",
                    sensor?.ZoneName ?? "-",
                    sensorEvent.OccurredAtUtc);
            })
            .ToArray();
    }

    private sealed record SensorDetails(Guid SensorId, string Name, string ZoneName);

    private sealed record SensorEventRow(
        Guid EventIdentifier,
        string SensorName,
        string ZoneName,
        DateTime OccurredAtUtc);
}
