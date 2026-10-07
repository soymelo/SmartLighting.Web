using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SmartLighting.Web.Features.Home.Pages;

public partial class Index : IAsyncDisposable
{
    [Inject] private NavigationManager Nav { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;

    private ElementReference heroElement;
    private ElementReference lightCanvas;
    private IJSObjectReference? lightModule;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        lightModule = await JS.InvokeAsync<IJSObjectReference>(
            "import", "./Features/Home/Pages/Index.razor.js");
        await lightModule.InvokeVoidAsync("startLightTrail", heroElement, lightCanvas);
    }

    private void OpenProjects() => Nav.NavigateTo("/projects");

    public async ValueTask DisposeAsync()
    {
        if (lightModule is null)
        {
            return;
        }

        try
        {
            await lightModule.InvokeVoidAsync("stopLightTrail", lightCanvas);
            await lightModule.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
            // O circuito pode ter sido encerrado antes da navegação terminar.
        }
    }
}
