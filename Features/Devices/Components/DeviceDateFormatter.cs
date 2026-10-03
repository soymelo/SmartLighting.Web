namespace SmartLighting.Web.Features.Devices.Components;

internal static class DeviceDateFormatter
{
    private static readonly TimeZoneInfo BrasiliaTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

    public static string Format(DateTime dateTimeUtc)
    {
        var utcDateTime = DateTime.SpecifyKind(dateTimeUtc, DateTimeKind.Utc);
        var brasiliaDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, BrasiliaTimeZone);

        return brasiliaDateTime.ToString("dd/MM/yyyy 'às' HH:mm");
    }
}
