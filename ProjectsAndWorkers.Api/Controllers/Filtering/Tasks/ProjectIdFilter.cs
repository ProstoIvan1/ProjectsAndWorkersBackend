using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering.Tasks
{
	public class ProjectIdFilter : Filter<TaskEntity, int?>
	{
		public ProjectIdFilter(int? value) : base(value)
		{
		}

		public override Expression<Func<TaskEntity, bool>>? GetExpression()
		{
			if (Value != null)
				return t => t.ProjectId == Value;
			else return null;
		}
	}
}
