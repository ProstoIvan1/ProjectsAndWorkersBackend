using ProjectsAndWorkers.Data.Models;

namespace ProjectsAndWorkers.Api.Controllers.Responses.Tasks
{
	public record GetTaskResponse(int Id, string Title, string? Description, TaskEntityStatus Status, int Priority, int? AuthorId, int? PerformerId, int ProjectId);
}
