namespace ProjectsAndWorkers.Api.Controllers.Requests.Projects
{
	public record UpdateProjectRequest(string Name, DateOnly StartDate, DateOnly EndDate, int Priority, string CustomerName, string PerformerName, int ManagerId);
}
