using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering.Projects
{
	public class ProjectMinPriorityFilter : Filter<Project, int?>
	{
		public ProjectMinPriorityFilter(int? value) : base(value)
		{
		}

		public override Expression<Func<Project, bool>>? GetExpression()
		{
			if (Value != null)
				return p => p.Priority >= Value;
			else return null;
		}
	}
}