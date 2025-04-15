using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Sorting.Projects
{
	public class EndDateProperty : IGetPropertyFuncStrategy<Project>
	{
		public string Name => "end date";

		public Expression<Func<Project, object>> GetExpression()
		{
			return p => p.EndDate;
		}
	}
}
