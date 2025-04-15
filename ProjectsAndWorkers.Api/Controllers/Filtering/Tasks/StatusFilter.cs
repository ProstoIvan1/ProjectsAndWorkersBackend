using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering.Tasks
{
	public class StatusFilter : Filter<TaskEntity, TaskEntityStatus?>
	{
		public StatusFilter(TaskEntityStatus? value) : base(value)
		{
		}

		public override Expression<Func<TaskEntity, bool>>? GetExpression()
		{
			if (Value != null)
				return t => t.Status == Value;
			else return null;
		}
	}
}
