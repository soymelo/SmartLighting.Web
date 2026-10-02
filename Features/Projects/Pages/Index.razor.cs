using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.Projects.Application;
using SmartLighting.Web.Features.Projects.Contracts.ViewModels;

namespace SmartLighting.Web.Features.Projects.Pages;

public partial class Index
{
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private ProjectService ProjectService { get; set; } = default!;

    private bool isLoading = true;
    private string? errorMessage;
    private IReadOnlyCollection<ProjectVMList> projects = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        isLoading = true;
        errorMessage = null;

        try
        {
            var result = await ProjectService.GetProjectsAsync();

            if (!result.Success)
            {
                errorMessage = result.Message ?? "Não foi possível carregar os projetos.";
                return;
            }

            projects = result.Data ?? [];
        }
        finally
        {
            isLoading = false;
        }
    }

    private void OpenDashboard(Guid projectId)
    {
        Nav.NavigateTo($"/projects/{projectId}/dashboard");
    }
}
