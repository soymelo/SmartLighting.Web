using System.Net;
using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.ProjectDashboard.Application;
using SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

namespace SmartLighting.Web.Features.ProjectDashboard.Pages;

public partial class Index
{
    private static readonly TimeSpan OnlineThreshold = TimeSpan.FromMinutes(5);
    // Posições dos componentes indicadas pela equipe na imagem de referência da maquete.
    private static readonly MapMarker[] SensorMarkers =
    [
        new(1, 22, 66, true), new(2, 52, 71, true), new(3, 83, 66, true),
        new(4, 82, 21, false), new(5, 52, 21, false), new(6, 20, 24, false)
    ];

    private static readonly MapMarker[] LuminaireMarkers =
    [
        new(1, 27, 63, false), new(2, 44, 65, false),
        new(3, 63, 65, false), new(4, 78, 63, false),
        new(5, 74, 44, true), new(6, 76, 27, true),
        new(7, 62, 30, true), new(8, 41, 30, true),
        new(9, 25, 27, true), new(10, 23, 44, true)
    ];

    // Identidades dos LEDs da maquete atual. O nome pode mudar sem deslocar o ponto no mapa.
    private static readonly Guid[] MaqueteLuminaireIds =
    [
        Guid.Parse("acfdadf5-f47d-418a-85e7-17f19638fcf9"),
        Guid.Parse("301a7031-c368-4613-96ee-1109697979ee"),
        Guid.Parse("8ecd0bfa-e9a5-448a-ba61-91e357bd6d58"),
        Guid.Parse("28eb8ffc-13ff-47c2-aae9-61dad4b4123d"),
        Guid.Parse("09479a2e-736d-40ba-b2f2-869de1a3c26f"),
        Guid.Parse("deef2975-ca16-4b5c-b68e-1f647971bbb2"),
        Guid.Parse("c86a34b9-ad07-4402-b5ae-e80d72e709cf"),
        Guid.Parse("f008e26c-deb6-49d9-b634-6713909af269"),
        Guid.Parse("e920b097-855e-4e7f-9f05-48d64781f5ab"),
        Guid.Parse("f03b7f1b-aa0c-4e93-bbb3-2388efd6e73f")
    ];

    private sealed record MapMarker(int Number, int X, int Y, bool LabelBelow);

    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private ProjectDashboardService ProjectDashboardService { get; set; } = default!;

    [Parameter] public Guid ProjectId { get; set; }

    private bool isLoading = true;
    private bool isNotFound;
    private string? errorMessage;
    private ProjectDashboardVMList? projectDashboardVM;
    private int? sensorEventsTotalCount;
    private bool sensorEventsLoadFinished;

    private int luminairesWithStateCount =>
        projectDashboardVM?.Luminaires.Count(luminaire => luminaire.BrightnessPercentage is not null) ?? 0;

    private DateTime? lastTelemetryAtUtc => projectDashboardVM?.Devices
        .Select(device => device.LastTelemetryAtUtc)
        .OrderByDescending(date => date)
        .FirstOrDefault();

    private int onlineDeviceCount => projectDashboardVM?.Devices.Count(device =>
        device.LastTelemetryAtUtc is not null &&
        DateTime.UtcNow - device.LastTelemetryAtUtc.Value <= OnlineThreshold) ?? 0;

    private int GetZoneEventCount(Guid zoneId) => projectDashboardVM?.ZoneActivity24h
        .FirstOrDefault(item => item.ZoneId == zoneId)?.EventCount ?? 0;

    private string FormatZoneEventCount(Guid zoneId)
    {
        var count = GetZoneEventCount(zoneId);
        return $"{count:N0} {(count == 1 ? "registro" : "registros")}";
    }

    private bool IsMaqueteMap => projectDashboardVM is { } dashboard &&
        string.Equals(dashboard.Name, "Maquete TCC", StringComparison.OrdinalIgnoreCase) &&
        Enumerable.Range(1, 6).All(number => dashboard.Zones.Any(zone =>
            zone.DisplayOrder == number && zone.SensorCount == 1)) &&
        Enumerable.Range(1, 10).All(number => GetMapLuminaire(number) is not null);

    private ProjectZoneVMList? GetMapZone(int sensorNumber) => projectDashboardVM?.Zones
        .FirstOrDefault(zone => zone.DisplayOrder == sensorNumber);

    private ProjectLuminaireVMList? GetMapLuminaire(int luminaireNumber)
    {
        var luminaires = projectDashboardVM?.Luminaires;
        if (luminaires is null) return null;

        var knownId = MaqueteLuminaireIds[luminaireNumber - 1];
        return luminaires.FirstOrDefault(luminaire => luminaire.LuminaireId == knownId)
            ?? luminaires.FirstOrDefault(luminaire => string.Equals(
                luminaire.Name, $"LED {luminaireNumber}", StringComparison.OrdinalIgnoreCase));
    }

    private static string FormatBrightness(ProjectLuminaireVMList luminaire) =>
        luminaire.BrightnessPercentage is int brightness ? $"{brightness}%" : "sem registro";

    private string GetZoneActivityClass(Guid zoneId)
    {
        var count = GetZoneEventCount(zoneId);
        var maximum = projectDashboardVM?.ZoneActivity24h.Select(item => item.EventCount).DefaultIfEmpty(0).Max() ?? 0;
        if (count == 0 || maximum == 0) return "activity-none";
        var ratio = (double)count / maximum;
        return ratio >= 0.75 ? "activity-high" : ratio >= 0.35 ? "activity-medium" : "activity-low";
    }

    protected override async Task OnParametersSetAsync() => await LoadData();

    private async Task LoadData()
    {
        isLoading = true;
        isNotFound = false;
        errorMessage = null;
        projectDashboardVM = null;
        sensorEventsTotalCount = null;
        sensorEventsLoadFinished = false;

        try
        {
            var result = await ProjectDashboardService.GetProjectDashboardAsync(ProjectId);

            if (!result.Success)
            {
                isNotFound = result.StatusCode == HttpStatusCode.NotFound;
                errorMessage = isNotFound
                    ? null
                    : result.Message ?? "Não foi possível carregar o dashboard do projeto.";
                return;
            }

            projectDashboardVM = result.Data;

            if (projectDashboardVM is null)
            {
                errorMessage = "A API não retornou os dados do projeto.";
                return;
            }

        }
        finally
        {
            isLoading = false;
        }
    }

    private void OpenDevice(Guid deviceId)
    {
        Nav.NavigateTo($"/devices/{deviceId}");
    }

    private void OnSensorEventsTotalCountChanged(int? count)
    {
        sensorEventsTotalCount = count;
        sensorEventsLoadFinished = true;
    }

    private void CreateDevice()
    {
        Nav.NavigateTo($"/projects/{ProjectId}/devices/new");
    }

}
