using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering.Projects
{
	public class StartDateRangeFilter : Filter<Project, DateOnly?>
	{
		public StartDateRangeFilter(DateOnly? value) : base(value)
		{
		}

		public override Expression<Func<Project, bool>>? GetExpression()
		{
			if (Value != null)
				return p => p.StartDate >= Value && p.EndDate >= Value;
			else return null;
		}
	}
}
