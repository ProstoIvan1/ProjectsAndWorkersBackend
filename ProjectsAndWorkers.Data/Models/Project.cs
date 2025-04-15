using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectsAndWorkers.Data.Models;

[Table("Project")]
public partial class Project : IIdentifiable, IPriority
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string PerformerName { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int Priority { get; set; }

    public int? ManagerId { get; set; }

    [ForeignKey("ManagerId")]
    [InverseProperty("ProjectsToManage")]
    public virtual Worker? ManagerNavigation { get; set; } = null!;

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectsNavigation")]
    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();

	public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
}
