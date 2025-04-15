using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectsAndWorkers.Api.Models
{
	public partial class Worker : IIdentifiable
	{
		[NotMapped]
		public string FullName => LastName + " " + FirstName + " " + Patronymic;
	}
}
