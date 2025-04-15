using ProjectsAndWorkers.Api.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Sorting
{
	public class StartDateProperty : IGetPropertyFuncStrategy<Project>
	{
		public string Name => "start date";

		public Expression<Func<Project, object>> GetExpression()
		{
			return p => p.StartDate;
		}
	}
}
