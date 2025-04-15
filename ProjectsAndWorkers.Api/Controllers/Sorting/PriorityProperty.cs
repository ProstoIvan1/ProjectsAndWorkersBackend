using ProjectsAndWorkers.Api.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Sorting
{
	public class PriorityProperty : IGetPropertyFuncStrategy<Project>
	{
		public string Name => "priority";

		public Expression<Func<Project, object>> GetExpression()
		{
			return p => p.Priority;
		}
	}
}
