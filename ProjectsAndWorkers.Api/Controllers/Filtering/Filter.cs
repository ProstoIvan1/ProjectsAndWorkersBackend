using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering
{
	public abstract class Filter<T, ValueT> : IFilter<T>
	{
		protected Filter(ValueT value)
		{
			Value = value;
		}

		public ValueT? Value { get; }

		public abstract Expression<Func<T, bool>> GetExpression();
	}
}
