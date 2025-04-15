using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Sorting.Tasks
{
	public class TaskPriorityProperty : IGetPropertyFuncStrategy<TaskEntity>
	{
		public string Name => "priority";

		public Expression<Func<TaskEntity, object>> GetExpression()
		{
			return t => t.Priority;
		}
	}
}
