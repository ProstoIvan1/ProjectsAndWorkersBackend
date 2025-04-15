using ProjectsAndWorkers.Api.Controllers.Responses.Projects;
using ProjectsAndWorkers.Api.Controllers.Responses.Tasks;
using ProjectsAndWorkers.Api.Controllers.Responses.Workers;
using ProjectsAndWorkers.Data.Models;

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

		public static GetTaskResponse ToResponse(this TaskEntity task)
		{
			return new GetTaskResponse(task.Id, task.Title, task.Description, task.Status, task.Priority, task.AuthorId, task.PerformerId, task.ProjectId);
		}
	}
}
