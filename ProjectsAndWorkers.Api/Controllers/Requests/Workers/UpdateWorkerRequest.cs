using ProjectsAndWorkers.Data.Models;

namespace ProjectsAndWorkers.Api.Controllers.Requests.Workers
{
	public record UpdateWorkerRequest(string FirstName, string LastName, string? Patronymic, string Mail);
}
