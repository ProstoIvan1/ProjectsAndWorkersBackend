using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering.Tasks
{
	public class PerformerIdFilter : Filter<TaskEntity, int?>
	{
		public PerformerIdFilter(int? value) : base(value)
		{
		}

		public override Expression<Func<TaskEntity, bool>>? GetExpression()
		{
			if (Value != null)
				return t => Value == null || t.PerformerId == Value;
			return null;
		}
	}
}
