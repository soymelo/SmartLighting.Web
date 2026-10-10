using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.MachineLearning.Application;
using SmartLighting.Web.Features.MachineLearning.Contracts.Dtos;

namespace SmartLighting.Web.Features.MachineLearning.Pages;

public partial class Experiments
{
    [Inject] private MlDemoService MlDemoService { get; set; } = default!;

    private bool isLoading = true;
    private string? errorMessage;
    private GetMlDemoDto? demo;

    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
        errorMessage = null;
        demo = null;

        try
        {
            var result = await MlDemoService.GetDemoAsync();
            if (!result.Success || result.Data is null)
            {
                errorMessage = "A demonstração de IA está indisponível. Inicie o serviço Python para visualizar os exemplos.";
                return;
            }

            demo = result.Data;
        }
        finally
        {
            isLoading = false;
        }
    }

    private static string DetectorName(string detector) => detector switch
    {
        "isolation_forest" => "Isolation Forest",
        "statistical_frequency" => "Comparação estatística",
        _ => detector,
    };

    private static string FormatWindow(DateTime utc)
    {
        var zone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), zone)
            .ToString("dd/MM/yyyy HH:mm");
    }
}
