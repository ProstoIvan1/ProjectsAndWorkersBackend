using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectsAndWorkers.Api.Models;

[Table("Worker")]
public partial class Worker : IIdentifiable
{
    [Key]
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Mail { get; set; } = null!;

    [InverseProperty("ManagerNavigation")]
    public virtual ICollection<Project> ProjectsToManage { get; set; } = new List<Project>();

    [ForeignKey("WorkerId")]
    [InverseProperty("Workers")]
    public virtual ICollection<Project> ProjectsNavigation { get; set; } = new List<Project>();
}
