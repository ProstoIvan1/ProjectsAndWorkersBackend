using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Sorting.Projects
{
	public class ProjectNameProperty : IGetPropertyFuncStrategy<Project>
	{
		public string Name => "name";

		public Expression<Func<Project, object>> GetExpression()
		{
			return p => p.Name;
		}
	}
}
