using ProjectsAndWorkers.Data.Models;

namespace ProjectsAndWorkers.Api.Controllers.Sorting.Tasks
{
	public class TaskStatusProperty : IGetPropertyFuncStrategy<TaskEntity>
	{
		public string Name => "status";

		public System.Linq.Expressions.Expression<Func<TaskEntity, object>> GetExpression()
		{
			return t => t.Status;
		}
	}
}
