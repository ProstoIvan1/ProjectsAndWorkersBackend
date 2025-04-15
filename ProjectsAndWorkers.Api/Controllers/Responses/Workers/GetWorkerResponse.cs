using ProjectsAndWorkers.Data.Models;

namespace ProjectsAndWorkers.Api.Controllers.Responses.Workers
{
	public record GetWorkerResponse(int Id, string FirstName, string LastName, string? Patronymic, string Mail);
}
