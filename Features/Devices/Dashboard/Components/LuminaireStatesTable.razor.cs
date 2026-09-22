using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.Devices.Dashboard.Contracts.Dtos;

namespace SmartLighting.Web.Features.Devices.Dashboard.Components;

public partial class LuminaireStatesTable
{
    [Parameter, EditorRequired]
    public required GetDeviceTopologyDto Topology { get; set; }

    [Parameter, EditorRequired]
    public required GetDeviceLuminaireStatesDto States { get; set; }

    private IReadOnlyCollection<LuminaireStateRow> rows = [];

    protected override void OnParametersSet()
    {
        var statesByLuminaireId = States.Luminaires.ToDictionary(state => state.LuminaireId);

        rows = Topology.Zones
            .OrderBy(zone => zone.DisplayOrder)
            .SelectMany(zone => zone.Luminaires.Select(luminaire =>
            {
                statesByLuminaireId.TryGetValue(luminaire.LuminaireId, out var state);

                return new LuminaireStateRow(
                    luminaire.Name,
                    zone.Name,
                    state?.BrightnessPercentage,
                    state?.UpdatedAtUtc);
            }))
            .ToArray();
    }

    private sealed record LuminaireStateRow(
        string LuminaireName,
        string ZoneName,
        int? BrightnessPercentage,
        DateTime? UpdatedAtUtc);
}
