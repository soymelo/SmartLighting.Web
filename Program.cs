using Radzen;
using SmartLighting.Web.Components;
using SmartLighting.Web.Features.DeviceDashboard.Application;
using SmartLighting.Web.Features.Devices.Application;
using SmartLighting.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRadzenComponents();
builder.Services.AddHttpClient<IApiClient, ApiClient>((services, httpClient) =>
{
    var baseUrl = services.GetRequiredService<IConfiguration>()["Api:BaseUrl"]
        ?? throw new InvalidOperationException("A configuração 'Api:BaseUrl' não foi definida.");

    httpClient.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
});

builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<DeviceDashboardService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
