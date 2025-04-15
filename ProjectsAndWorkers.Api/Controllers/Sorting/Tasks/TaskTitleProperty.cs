using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Sorting.Tasks
{
	public class TaskTitleProperty : IGetPropertyFuncStrategy<TaskEntity>
	{
		public string Name => "title";

		public Expression<Func<TaskEntity, object>> GetExpression()
		{
			return t => t.Title;
		}
	}
}
