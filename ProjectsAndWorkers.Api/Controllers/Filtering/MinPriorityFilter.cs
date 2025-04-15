using ProjectsAndWorkers.Api.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering
{
	public class MinPriorityFilter : Filter<Project, int>
	{
		public MinPriorityFilter(int value) : base(value)
		{
		}

		public override Expression<Func<Project, bool>> GetExpression()
		{
			return p => p.Priority >= Value;
		}
	}
}