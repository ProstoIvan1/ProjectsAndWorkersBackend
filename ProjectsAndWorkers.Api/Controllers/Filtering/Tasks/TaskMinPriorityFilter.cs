using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering.Tasks
{
	public class TaskMinPriorityFilter : Filter<TaskEntity, int?>
	{
		public TaskMinPriorityFilter(int? value) : base(value)
		{
		}

		public override Expression<Func<TaskEntity, bool>>? GetExpression()
		{
			if (Value != null)
				return p => p.Priority >= Value;
			else return null;
		}
	}
}
