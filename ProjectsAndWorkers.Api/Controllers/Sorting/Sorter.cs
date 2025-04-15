namespace ProjectsAndWorkers.Api.Controllers.Sorting
{
	public class Sorter<T> where T : class
	{
		public Sorter(params IGetPropertyFuncStrategy<T>[] strategies)
		{
			Strategies = strategies.ToList();
		}

		public List<IGetPropertyFuncStrategy<T>> Strategies { get; }

		public IQueryable<T> Sort(ref IQueryable<T> query, string? itemName, bool isDesc = false)
		{
			foreach (var strategy in Strategies)
			{
				if (itemName == strategy.Name)
				{
					if (isDesc) query = query.OrderByDescending(strategy.GetExpression());
					else query = query.OrderBy(strategy.GetExpression());

					return query;
				}
			}

			return query;
		}
	}
}
