using ProjectFlow.Domain.Common;

namespace ProjectFlow.Domain;

public class Project : BaseEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateTime CreationDate { get; private set; }
    public DateTime? DueDate { get; private set; }

    private readonly List<TaskItem> _tasks = new List<TaskItem>();
    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

    private Project() { }

    public static Project Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Project name cannot be empty.", nameof(name));
        }

        var project = new Project
        {
            Name = name,
            CreationDate = DateTime.UtcNow,
            DueDate = null 
        };

        return project;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("Project name cannot be empty.", nameof(newName));
        }
        this.Name = newName;
    }

    public void UpdateDueDateFromTasks()
    {
        if (_tasks.Any(t => t.DueDate.HasValue))
        {
            this.DueDate = _tasks.Where(t => t.DueDate.HasValue).Max(t => t.DueDate);
        }
        else
        {
            this.DueDate = null;
        }
    }
}