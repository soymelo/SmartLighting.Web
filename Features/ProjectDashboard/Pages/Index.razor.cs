using System.Net;
using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.ProjectDashboard.Application;
using SmartLighting.Web.Features.ProjectDashboard.Contracts.ViewModels;

namespace SmartLighting.Web.Features.ProjectDashboard.Pages;

public partial class Index
{
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private ProjectDashboardService ProjectDashboardService { get; set; } = default!;

    [Parameter] public Guid ProjectId { get; set; }

    private bool isLoading = true;
    private bool isNotFound;
    private string? errorMessage;
    private ProjectDashboardVMList? projectDashboardVM;

    private int luminairesWithStateCount =>
        projectDashboardVM?.Luminaires.Count(luminaire => luminaire.BrightnessPercentage is not null) ?? 0;

    private DateTime? lastTelemetryAtUtc => projectDashboardVM?.Devices
        .Select(device => device.LastTelemetryAtUtc)
        .OrderByDescending(date => date)
        .FirstOrDefault();

    protected override async Task OnParametersSetAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        isLoading = true;
        isNotFound = false;
        errorMessage = null;
        projectDashboardVM = null;

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

    private void CreateDevice()
    {
        Nav.NavigateTo($"/projects/{ProjectId}/devices/new");
    }
}
