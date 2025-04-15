using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;

namespace ProjectsAndWorkers.Api.Controllers.Filtering.Tasks
{
	public class AuthorIdFilter : Filter<TaskEntity, int?>
	{
		public AuthorIdFilter(int? value) : base(value)
		{
		}

		public override Expression<Func<TaskEntity, bool>>? GetExpression()
		{
			if (Value != null)
				return t => t.AuthorId == Value;

			else return null;
		}
	}
}
