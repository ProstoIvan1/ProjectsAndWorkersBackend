using ProjectsAndWorkers.Api.Models;

namespace ProjectsAndWorkers.Api.Controllers.Requests
{
	public record UpdateWorkerRequest(string FirstName, string LastName, string Patronymic, string Mail);
}
