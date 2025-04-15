using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Sorting.Projects
{
	public class ProjectPriorityProperty : IGetPropertyFuncStrategy<Project>
	{
		public string Name => "priority";

		public Expression<Func<Project, object>> GetExpression()
		{
			return p => p.Priority;
		}
	}
}
