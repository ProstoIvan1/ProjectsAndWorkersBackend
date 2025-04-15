namespace ProjectsAndWorkers.Api.Controllers.Requests.Projects
{
	public record CreateProjectRequest(string Name, DateOnly StartDate, DateOnly EndDate, int Priority, string CustomerName, string PerformerName, int ManagerId, int[] WorkerIds);
}
