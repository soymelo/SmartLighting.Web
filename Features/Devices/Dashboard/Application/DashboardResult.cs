using SmartLighting.Web.Features.Devices.Dashboard.Contracts.Dtos;
using SmartLighting.Web.Infrastructure;

namespace SmartLighting.Web.Features.Devices.Dashboard.Application;

public sealed record DashboardResult(
    ApiResponse<GetDeviceTopologyDto> Topology,
    ApiResponse<GetDeviceTelemetryStatusDto> TelemetryStatus,
    ApiResponse<GetDeviceLuminaireStatesDto> LuminaireStates,
    ApiResponse<GetRecentSensorEventsDto> RecentSensorEvents);
