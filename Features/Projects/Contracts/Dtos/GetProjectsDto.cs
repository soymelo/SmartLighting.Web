namespace SmartLighting.Web.Features.Projects.Contracts.Dtos;

public sealed record GetProjectsDto(IReadOnlyCollection<ProjectDto> Projects);

public sealed record ProjectDto(Guid ProjectId, string Name);
