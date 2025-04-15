using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering.Projects
{
	public class ProjectMaxPriorityFilter : Filter<Project, int?>
	{
		public ProjectMaxPriorityFilter(int? value) : base(value)
		{
		}

		public override Expression<Func<Project, bool>>? GetExpression()
		{
			if (Value != null)
				return p => p.Priority <= Value;
			else return null;
		}
	}
}
