using ProjectsAndWorkers.Api.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Sorting
{
	public class IdProperty : IGetPropertyFuncStrategy<IIdentifiable>
	{
		public string Name => "id";

		public Expression<Func<IIdentifiable, object>> GetExpression()
		{
			return entity => entity.Id;
		}
	}
}
