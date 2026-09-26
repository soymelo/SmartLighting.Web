using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text.Json;

namespace SmartLighting.Web.Infrastructure;

public interface IApiClient
{
    Task<ApiResponse<TResponse>> GetAsync<TResponse>(string endpoint, IDictionary<string, string?>? parameters = null, CancellationToken ct = default);

    Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest content, CancellationToken ct = default);

    Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest content, CancellationToken ct = default);

    Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string endpoint, CancellationToken ct = default);
}

public sealed class ApiClient(HttpClient httpClient) : IApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task<ApiResponse<TResponse>> GetAsync<TResponse>(
        string endpoint, 
        IDictionary<string, string?>? parameters = null,
        CancellationToken ct = default)
    {
        var requestUri = parameters is null || parameters.Count == 0
            ? endpoint
            : QueryHelpers.AddQueryString(endpoint, parameters);

        return SendAsync<TResponse>(HttpMethod.Get, requestUri, ct: ct);
    }

    public Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest content,
        CancellationToken ct = default)
    {
        return SendAsync<TResponse>(
            HttpMethod.Post,
            endpoint,
            JsonContent.Create(content, options: JsonOptions),
            ct);
    }

    public Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(
        string endpoint,
        TRequest content,
        CancellationToken ct = default)
    {
        return SendAsync<TResponse>(
            HttpMethod.Put,
            endpoint,
            JsonContent.Create(content, options: JsonOptions),
            ct);
    }

    public Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(
        string endpoint,
        CancellationToken ct = default)
    {
        return SendAsync<TResponse>(HttpMethod.Delete, endpoint, ct: ct);
    }

    private async Task<ApiResponse<TResponse>> SendAsync<TResponse>(
        HttpMethod method,
        string endpoint,
        HttpContent? content = null,
        CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(method, endpoint)
        {
            Content = content
        };

        try
        {
            using var response = await httpClient.SendAsync(request, ct);
            var responseContent = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = GetErrorMessage(
                    response.StatusCode,
                    responseContent,
                    response.ReasonPhrase);

                return ApiResponse<TResponse>.Failure(response.StatusCode, errorMessage);
            }

            var data = string.IsNullOrWhiteSpace(responseContent)
                ? default
                : JsonSerializer.Deserialize<TResponse>(responseContent, JsonOptions);

            return ApiResponse<TResponse>.Success(response.StatusCode, data);
        }
        catch (HttpRequestException)
        {
            return ApiResponse<TResponse>.Failure(
                null,
                "Não foi possível comunicar com a API.");
        }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested)
        {
            return ApiResponse<TResponse>.Failure(
                null,
                "A API demorou mais do que o esperado para responder.");
        }
        catch (JsonException)
        {
            return ApiResponse<TResponse>.Failure(
                null,
                "A API retornou uma resposta inválida.");
        }
    }

    private static string GetErrorMessage(
        HttpStatusCode statusCode,
        string responseContent,
        string? reasonPhrase)
    {
        if (!string.IsNullOrWhiteSpace(responseContent))
        {
            try
            {
                var error = JsonSerializer.Deserialize<ApiErrorDto>(responseContent, JsonOptions);

                if (!string.IsNullOrWhiteSpace(error?.Detail))
                {
                    return error.Detail;
                }

                if (!string.IsNullOrWhiteSpace(error?.Title))
                {
                    return error.Title;
                }

                if (!string.IsNullOrWhiteSpace(error?.Message))
                {
                    return error.Message;
                }
            }
            catch (JsonException)
            {
            }
        }

        return statusCode switch
        {
            HttpStatusCode.BadRequest => "A solicitação possui dados inválidos.",
            HttpStatusCode.NotFound => "O recurso solicitado não foi encontrado.",
            HttpStatusCode.Conflict => "Não foi possível concluir a operação devido ao estado atual do recurso.",
            HttpStatusCode.UnprocessableEntity => "A solicitação não pôde ser processada.",
            HttpStatusCode.InternalServerError => "Ocorreu um erro inesperado ao processar a solicitação.",
            HttpStatusCode.ServiceUnavailable => "A API está indisponível no momento.",
            _ => reasonPhrase ?? "A API retornou um erro ao processar a solicitação."
        };
    }

    private sealed record ApiErrorDto(string? Title, string? Detail, string? Message);
}

public sealed record ApiResponse<T>(
    bool IsSuccess,
    HttpStatusCode? StatusCode,
    T? Data,
    string? ErrorMessage)
{
    public static ApiResponse<T> Success(HttpStatusCode statusCode, T? data) =>
        new(true, statusCode, data, null);

    public static ApiResponse<T> Failure(HttpStatusCode? statusCode, string errorMessage) =>
        new(false, statusCode, default, errorMessage);
}
