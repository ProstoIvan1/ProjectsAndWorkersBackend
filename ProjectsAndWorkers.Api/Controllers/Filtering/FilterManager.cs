namespace ProjectsAndWorkers.Api.Controllers.Filtering
{
	public class FilterManager<T>
	{
		public FilterManager(params IFilter<T>[] filters)
		{
			Filters = filters.ToList();
		}

		public List<IFilter<T>> Filters { get; } 

		public IQueryable<T> Filter(ref IQueryable<T> query)
		{
			foreach (var filter in Filters)
			{
				var expression = filter.GetExpression();

				if (expression != null)
					query = query.Where(expression);
			}

			return query;
		}
	}
}
