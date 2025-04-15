using ProjectsAndWorkers.Api.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering
{
	public class EndDateRangeFilter : Filter<Project, DateOnly>
	{
		public EndDateRangeFilter(DateOnly value) : base(value)
		{
		}

		public override Expression<Func<Project, bool>> GetExpression()
		{
			return p => p.StartDate <= Value && p.EndDate <= Value;
		}
	}
}
