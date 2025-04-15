using ProjectsAndWorkers.Api.Models;

namespace ProjectsAndWorkers.Api.Controllers.Responses
{
	public record GetWorkerResponse(int Id, string FirstName, string LastName, string Patronymic, string Mail);
}
