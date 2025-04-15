using ProjectsAndWorkers.Data.Models;

namespace ProjectsAndWorkers.Api.Controllers.Requests.Tasks
{
	public record GetTasksRequest(string? SortItem, string? SortDirection, int? MaxPriority, int? MinPriority, TaskEntityStatus? StatusFilter, int? AuthorFilter, int? PerformerFilter, int? ProjectFilter);
}
