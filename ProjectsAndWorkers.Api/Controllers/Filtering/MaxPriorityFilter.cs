using ProjectsAndWorkers.Api.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering
{
	public class MaxPriorityFilter : Filter<Project, int>
	{
		public MaxPriorityFilter(int value) : base(value)
		{
		}

		public override Expression<Func<Project, bool>> GetExpression()
		{
			return p => p.Priority <= Value;
		}
	}
}
