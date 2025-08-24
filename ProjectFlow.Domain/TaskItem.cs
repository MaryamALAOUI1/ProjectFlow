using ProjectFlow.Domain.Common;
using ProjectFlow.Domain.Events; 
namespace ProjectFlow.Domain;

public class TaskItem : BaseEntity
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TaskStatus Status { get; private set; } = TaskStatus.ToDo;
    public DateTime? DueDate { get; private set; }
    public int ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;

    private TaskItem() { }

    public static TaskItem Create(string title, int projectId, string? description = null, DateTime? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Task title cannot be empty.", nameof(title));
        }

        var taskItem = new TaskItem
        {
            Title = title,
            ProjectId = projectId,
            Description = description,
            DueDate = dueDate,
            Status = TaskStatus.ToDo
        };

        return taskItem;
    }

    public void Complete()
    {
        this.Status = TaskStatus.Done;
        this.AddDomainEvent(new TaskCompletedEvent(this));
    }

    public void UpdateDetails(string newTitle, string? newDescription)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new ArgumentException("Task title cannot be empty.", nameof(newTitle));
        }
        this.Title = newTitle;
        this.Description = newDescription;
    }
    public void UpdateStatus(TaskStatus newStatus)
    {
        this.Status = newStatus;
        this.AddDomainEvent(new TaskUpdatedEvent(this));
    }

}