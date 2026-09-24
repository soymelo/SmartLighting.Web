namespace SmartLighting.Web.Infrastructure;

public abstract class ServiceBase
{
    protected ServiceResult<T> FromApiResponse<T>(ApiResponse<T> response)
    {
        return response.IsSuccess
            ? ServiceResult<T>.Ok(response.StatusCode, response.Data)
            : ServiceResult<T>.Failure(response.StatusCode, GetErrorMessage(response));
    }

    protected ServiceResult<TOutput> FromApiResponse<TInput, TOutput>(
        ApiResponse<TInput> response,
        Func<TInput, TOutput> map)
    {
        if (!response.IsSuccess)
        {
            return ServiceResult<TOutput>.Failure(response.StatusCode, GetErrorMessage(response));
        }

        var data = response.Data is null ? default : map(response.Data);

        return ServiceResult<TOutput>.Ok(response.StatusCode, data);
    }

    private static string GetErrorMessage<T>(ApiResponse<T> response)
    {
        return response.ErrorMessage ?? "Não foi possível processar a requisição.";
    }
}
