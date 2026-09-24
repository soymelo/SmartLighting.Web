using SmartLighting.Web.Features.Devices.Contracts.Dtos;
using SmartLighting.Web.Infrastructure;

using SmartLighting.Web.Features.Devices.Contracts.ViewModels;

namespace SmartLighting.Web.Features.Devices.Application;

public class DeviceService(IApiClient apiClient) : ServiceBase
{
    public async Task<ServiceResult<IReadOnlyCollection<DeviceVMList>>> GetDevicesAsync()
    {
        const string endpoint = "/devices";

        var result = await apiClient.GetAsync<GetDevicesDto>(endpoint);

        return FromApiResponse(
            result,
            X => (IReadOnlyCollection<DeviceVMList>)X.Devices
                .Select(d => new DeviceVMList(
                    d.DeviceId,
                    d.Name,
                    d.LastTelemetryAtUtc))
                .ToArray());
    }
}
