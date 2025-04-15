using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace ProjectsAndWorkers.Data.Models;

[Table("Worker")]
public partial class Worker : IIdentifiable
{
    [Key]
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    [AllowNull]
    public string? Patronymic { get; set; }

	[NotMapped]
	public string FullName => LastName + " " + FirstName + Patronymic == null ? "" : " " + Patronymic ;

	public string Mail { get; set; } = null!;

    [InverseProperty("ManagerNavigation")]
    public virtual ICollection<Project> ProjectsToManage { get; set; } = new List<Project>();

    [ForeignKey("WorkerId")]
    [InverseProperty("Workers")]
    public virtual ICollection<Project> ProjectsNavigation { get; set; } = new List<Project>();

	public ICollection<TaskEntity> CreatedTasks { get; set; } = new List<TaskEntity>();

	public ICollection<TaskEntity> PerformingTasks { get; set; } = new List<TaskEntity>();
}
