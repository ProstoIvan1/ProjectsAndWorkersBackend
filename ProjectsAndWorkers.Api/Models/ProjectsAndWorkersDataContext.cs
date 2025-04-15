using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProjectsAndWorkers.Api.Models;

public partial class ProjectsAndWorkersDataContext : DbContext
{
    private IConfiguration _configuration;

    public ProjectsAndWorkersDataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ProjectsAndWorkersDataContext(IConfiguration configuration, DbContextOptions<ProjectsAndWorkersDataContext> options)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite(_configuration.GetConnectionString("SQLite"));

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
