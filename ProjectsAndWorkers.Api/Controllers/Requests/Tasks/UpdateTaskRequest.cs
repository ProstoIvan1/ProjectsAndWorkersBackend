using ProjectsAndWorkers.Data.Models;

namespace ProjectsAndWorkers.Api.Controllers.Requests.Tasks
{
	public record UpdateTaskRequest(string Title, string? Description, TaskEntityStatus Status, int Priority, int AuthorId, int? PerformerId, int ProjectId);
}
