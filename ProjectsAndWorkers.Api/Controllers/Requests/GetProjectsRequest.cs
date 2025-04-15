namespace ProjectsAndWorkers.Api.Controllers.Requests
{
	public record GetProjectsRequest(string? SortItem, string? SortDirection, int? MinPriority, int? MaxPriority, DateOnly? StartDateRange, DateOnly? EndDateRange);
}
