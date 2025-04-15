using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectsAndWorkers.Data.Models;

namespace ProjectsAndWorkers.Data;

public partial class ProjectsAndWorkersDataContext : DbContext
{

    public ProjectsAndWorkersDataContext( DbContextOptions<ProjectsAndWorkersDataContext> options)
		: base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }
	public virtual DbSet<TaskEntity> Tasks { get; set; }
	public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasOne(d => d.ManagerNavigation).WithMany(p => p.ProjectsToManage).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(d => d.Workers).WithMany(p => p.ProjectsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectWorker",
                    r => r.HasOne<Worker>().WithMany()
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Project>().WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("ProjectId", "WorkerId");
                        j.ToTable("Project_Worker");
                    });
        });

        modelBuilder.Entity<TaskEntity>(entity =>
        {
            entity
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId);

            entity
            .HasOne(t => t.Author)
            .WithMany(w => w.CreatedTasks)
            .HasForeignKey(t => t.AuthorId)
			.OnDelete(DeleteBehavior.SetNull);

            entity
            .HasOne(t => t.Performer)
            .WithMany(w => w.PerformingTasks)
            .HasForeignKey(t => t.PerformerId)
            .OnDelete(DeleteBehavior.SetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
