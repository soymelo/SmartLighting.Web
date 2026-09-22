namespace SmartLighting.Web.Features.Dashboard.Components;

internal static class DashboardDateFormatter
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
