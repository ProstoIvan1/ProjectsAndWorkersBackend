using Microsoft.EntityFrameworkCore;
using ProjectsAndWorkers.Data;

namespace ProjectsAndWorkers.Data.Models
{
	public static class IdExtensions
	{

		public static async Task<int?> GetIncorrectId<T>(this DbSet<T> dbSet, CancellationToken ct, params int[] ids) where T : class, IIdentifiable
		{
			int[] DbIds = await dbSet
				.Select(e => e.Id)
				.Where(id => ids.Contains(id))
				.ToArrayAsync(ct);

			foreach (var id in ids)
			{
				if (!DbIds.Contains(id))
					return id;
			}

			return null;
		}

		public static async Task<bool> IsExists<T>(this DbSet<T> dbSet, int id, CancellationToken ct) where T : class, IIdentifiable
		{
			return await dbSet.Where(t => t.Id == id).AnyAsync(ct);
		}
	}
}
