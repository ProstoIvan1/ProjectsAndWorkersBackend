using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Sorting.Projects
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
