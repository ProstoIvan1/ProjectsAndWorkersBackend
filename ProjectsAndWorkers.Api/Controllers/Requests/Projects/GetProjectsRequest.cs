namespace ProjectsAndWorkers.Api.Controllers.Requests.Projects
{
	public record GetProjectsRequest(string? SortItem, string? SortDirection, int? MinPriority, int? MaxPriority, DateOnly? StartDateRange, DateOnly? EndDateRange);
}
