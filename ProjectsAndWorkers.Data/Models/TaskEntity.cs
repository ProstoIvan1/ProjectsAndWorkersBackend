using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectsAndWorkers.Data.Models
{
	[Table("Task")]
	public class TaskEntity : IIdentifiable, IPriority
	{
		[Key]
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public string? Description { get; set; }
		public TaskEntityStatus Status { get; set; }
		public int Priority { get; set; }

		public int? AuthorId { get; set; }
		public int? PerformerId { get; set; }
		public int ProjectId { get; set; }

		[ForeignKey(nameof(AuthorId))]
		public Worker? Author { get; set; }
		
		[ForeignKey(nameof(PerformerId))]
		public Worker? Performer { get; set; }

		[ForeignKey(nameof(ProjectId))]
		public Project? Project { get; init; }

	}
}
