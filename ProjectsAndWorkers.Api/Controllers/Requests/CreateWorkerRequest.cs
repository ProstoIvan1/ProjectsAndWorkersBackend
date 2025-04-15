using ProjectsAndWorkers.Api.Models;

namespace ProjectsAndWorkers.Api.Controllers.Requests
{
	public record CreateWorkerRequest(string FirstName, string LastName, string Patronymic, string Mail);
}
