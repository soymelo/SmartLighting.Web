using SmartLighting.Web.Features.MachineLearning.Contracts.Dtos;
using SmartLighting.Web.Infrastructure;

namespace SmartLighting.Web.Features.MachineLearning.Application;

public sealed class MlDemoService(IApiClient apiClient) : ServiceBase
{
    public async Task<ServiceResult<GetMlDemoDto>> GetDemoAsync()
    {
        var response = await apiClient.GetAsync<GetMlDemoDto>("/ml/demo");
        return FromApiResponse(response);
    }
}
