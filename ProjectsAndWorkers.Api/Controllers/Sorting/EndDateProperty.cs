using ProjectsAndWorkers.Api.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Sorting
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
