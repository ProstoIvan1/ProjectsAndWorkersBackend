using Microsoft.EntityFrameworkCore;

namespace ProjectsAndWorkers.Api.Models
{
	public static class IdExtensions
	{
		public static async Task<int> GetIncorrectProjectId(this ProjectsAndWorkersDataContext dataContext, params int[] ids)
		{
			int[] allIds = await dataContext.Projects.Select(e => e.Id).ToArrayAsync();

			foreach (var id in ids)
			{
				if (!allIds.Contains(id))
					return id;
			}

			return 0;
		}

		public static async Task<int?> GetIncorrectWorkerId(this ProjectsAndWorkersDataContext dataContext, params int[] ids)
		{
			int[] allIds = await dataContext.Workers.Select(e => e.Id).ToArrayAsync();

			foreach (var id in ids)
			{
				if (!allIds.Contains(id))
					return id;
			}

			return null;
		}
	}
}
