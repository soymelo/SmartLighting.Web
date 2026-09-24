using System.Net;

namespace SmartLighting.Web.Infrastructure;

public sealed record ServiceResult<T>(
    bool Success,
    HttpStatusCode? StatusCode,
    T? Data,
    string? Message)
{
    public static ServiceResult<T> Ok(HttpStatusCode? statusCode, T? data) =>
        new(true, statusCode, data, null);

    public static ServiceResult<T> Failure(HttpStatusCode? statusCode, string message) =>
        new(false, statusCode, default, message);
}
