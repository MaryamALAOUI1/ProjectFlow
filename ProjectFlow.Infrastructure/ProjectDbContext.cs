using Microsoft.EntityFrameworkCore;
using ProjectFlow.Domain;

namespace ProjectFlow.Infrastructure;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
    {
    }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>()
            .Property(t => t.Status) 
            .HasConversion<string>(); 
    }
}