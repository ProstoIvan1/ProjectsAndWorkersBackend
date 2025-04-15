namespace ProjectsAndWorkers.Api.Controllers.Responses
{
	public record GetProjectResponse(int Id, string Name, DateOnly StartDate, DateOnly EndDate, int Priority, string CustomerName, string PerformerName, int? ManagerId);
}
