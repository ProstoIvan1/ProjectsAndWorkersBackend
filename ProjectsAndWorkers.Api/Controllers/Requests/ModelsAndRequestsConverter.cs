using ProjectsAndWorkers.Api.Models;

namespace ProjectsAndWorkers.Api.Controllers.Requests
{
	public static class ModelsAndRequestsConverter
	{
		public static Project ToProject(this CreateProjectRequest request)
		{
			var (name, startDate, endDate, priority, customerName, performerName, managerId, workerIds) = request;

			Project project = new()
			{
				Name = name,
				CustomerName = customerName,
				PerformerName = performerName,
				StartDate = startDate,
				EndDate = endDate,
				Priority = priority,
				ManagerId = managerId
			};

			foreach (var id in workerIds)
			{
				project.Workers.Add(new Worker() { Id = id });
			}

			return project;
		}

		public static Worker ToWorker(this CreateWorkerRequest request)
		{
			var (firstName, lastName, patronymic, mail) = request;

			return new Worker()
			{
				FirstName = firstName,
				LastName = lastName,
				Patronymic = patronymic,
				Mail = mail
			};
		}
	}
}
