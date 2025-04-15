using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering.Tasks
{
	public class TaskMaxPriorityFilter : Filter<TaskEntity, int?>
	{
		public TaskMaxPriorityFilter(int? value) : base(value)
		{
		}

		public override Expression<Func<TaskEntity, bool>>? GetExpression()
		{
			if (Value != null)
				return p => Value == null || p.Priority <= Value;
			else return null;
		}
	}
}
