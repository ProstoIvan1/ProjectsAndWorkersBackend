using ProjectsAndWorkers.Api.Models;

namespace ProjectsAndWorkers.Api.Controllers.Responses
{
	public static class ModelsAndResponsesConverter
	{
		public static GetProjectResponse ToResponse(this Project project)
		{
			return new GetProjectResponse(project.Id, project.Name, project.StartDate, project.EndDate, project.Priority, project.CustomerName, project.PerformerName, project.ManagerId);
		}

		public static GetWorkerResponse ToResponse(this Worker worker)
		{
			return new GetWorkerResponse(worker.Id, worker.FirstName, worker.LastName, worker.Patronymic, worker.Mail);
		}
	}
}
