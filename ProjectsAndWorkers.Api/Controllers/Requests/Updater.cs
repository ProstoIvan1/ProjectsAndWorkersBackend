using ProjectsAndWorkers.Api.Models;

namespace ProjectsAndWorkers.Api.Controllers.Requests
{
	public static class Updater
	{
		public static void Update(this Project project, UpdateProjectRequest request)
		{
			var (name, startDate, endDate, priority, customerName, performerName, managerId) = request;

			project.Name = name;
			project.StartDate = startDate;
			project.EndDate = endDate;
			project.CustomerName = customerName;
			project.PerformerName = performerName;
			project.Priority = priority;
			project.ManagerId = managerId;
		}

		public static void Update(this Worker worker, UpdateWorkerRequest request)
		{
			var (firstName, lastName, patronymic, mail) = request;

			worker.FirstName = firstName;
			worker.LastName = lastName;
			worker.Patronymic = patronymic;
			worker.Mail = mail;
		}
	}
}
