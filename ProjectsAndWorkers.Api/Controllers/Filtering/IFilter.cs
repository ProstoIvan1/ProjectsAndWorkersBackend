using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering
{
	public interface IFilter<T>
	{
		Expression<Func<T, bool>>? GetExpression();
	}
}
