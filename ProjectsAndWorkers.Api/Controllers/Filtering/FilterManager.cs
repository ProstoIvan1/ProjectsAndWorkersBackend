namespace ProjectsAndWorkers.Api.Controllers.Filtering
{
	public class FilterManager<T>
	{
		public FilterManager(IEnumerable<IFilter<T>> filters)
		{
			Filters = filters.ToList();
		}

		public List<IFilter<T>> Filters { get; } 

		public IQueryable<T> Filter(ref IQueryable<T> query)
		{
			foreach (var filter in Filters)
			{
				query = query.Where(filter.GetExpression());
			}

			return query;
		}
	}
}
