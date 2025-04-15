using ProjectsAndWorkers.Api.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Sorting
{
	public class NameProperty : IGetPropertyFuncStrategy<Project>
	{
		public string Name => "name";

		public Expression<Func<Project, object>> GetExpression()
		{
			return p => p.Name;
		}
	}
}
