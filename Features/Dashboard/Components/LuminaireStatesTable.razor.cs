using Microsoft.AspNetCore.Components;
using SmartLighting.Web.Features.Dashboard.Contracts.ViewModels;

namespace SmartLighting.Web.Features.Dashboard.Components;

public partial class LuminaireStatesTable
{
    [Parameter, EditorRequired]
    public required IReadOnlyCollection<LuminaireStateVMList> Luminaires { get; set; }
}
