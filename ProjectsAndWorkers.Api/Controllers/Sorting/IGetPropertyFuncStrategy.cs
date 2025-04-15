using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Sorting
{
	public interface IGetPropertyFuncStrategy<T> where T : class
	{
		string Name { get; }

		Expression<Func<T, object>> GetExpression();
	}
}
