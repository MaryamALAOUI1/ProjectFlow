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
}