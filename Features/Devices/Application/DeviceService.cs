using SmartLighting.Web.Features.Devices.Contracts.Dtos;
using SmartLighting.Web.Infrastructure;

namespace SmartLighting.Web.Features.Devices.Application;

public class DeviceService(IApiClient apiClient)
{
    public async Task<ApiResponse<GetDevicesDto>> GetDevicesAsync()
    {
        var endpoint = $"/devices";

        return await apiClient.GetAsync<GetDevicesDto>(endpoint);
    }
}
