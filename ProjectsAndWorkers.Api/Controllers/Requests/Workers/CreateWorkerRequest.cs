using ProjectsAndWorkers.Data.Models;

namespace ProjectsAndWorkers.Api.Controllers.Requests.Workers
{
	public record CreateWorkerRequest(string FirstName, string LastName, string? Patronymic, string Mail);
}
